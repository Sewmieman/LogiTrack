using Asp.Versioning;
using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Customers.Commands.CreateCustomer;
using LogiTrack.Application.Customers.Queries.GetCustomerById;
using LogiTrack.Application.Customers.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using LogiTrack.Api;

namespace LogiTrack.Api.Controllers.V2;

[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/customers")]
public class CustomersV2Controller : ControllerBase
{
    private readonly ISender _sender;
    private readonly IApplicationDbContext _context;
    private readonly IIdempotencyStore _idempotency;
    public CustomersV2Controller(ISender sender, IApplicationDbContext context, IIdempotencyStore idempotency) => (_sender, _context, _idempotency) = (sender, context, idempotency);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        page = Math.Max(1, page); pageSize = Math.Clamp(pageSize, 1, 50);
        var q = _context.Customers.AsNoTracking();
        var total = await q.CountAsync(ct);
        var rows = await q.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new CustomerDto(x.Id,x.FirstName,x.LastName,x.Phone,x.Email,x.Address,x.CreatedAt)).ToListAsync(ct);
        var pages = (int)Math.Ceiling(total / (double)pageSize);
        return Ok(new { data=rows, meta=new { totalCount=total,page,pageSize,totalPages=pages,hasNext=page<pages,hasPrevious=page>1 }, links=new {
            self=$"/api/v2/customers?page={page}&pageSize={pageSize}", next=page<pages?$"/api/v2/customers?page={page+1}&pageSize={pageSize}":null, prev=page>1?$"/api/v2/customers?page={page-1}&pageSize={pageSize}":null }});
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetCustomerByIdQuery(id), ct);
        return result is null ? NotFound(new ProblemDetails { Title="Customer not found", Detail=$"No customer with id '{id}'.", Status=404 }) : Ok(result);
    }

    [HttpPost]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Create(CreateCustomerCommand command, [FromHeader(Name="Idempotency-Key")] string? key, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(key) && _idempotency.TryGet("customers", key, out var cached)) return StatusCode(cached.StatusCode, cached.Body);
        var result = await _sender.Send(command, ct);
        var response = new { data = result, links = new { self = $"/api/v2/customers/{result.Id}" } };
        if (!string.IsNullOrWhiteSpace(key)) _idempotency.TryAdd("customers", key, new IdempotencyResult(201,response));
        return CreatedAtAction(nameof(GetById), new { id=result.Id, version="2.0" }, response);
    }
}
