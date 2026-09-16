using Asp.Versioning;
using LogiTrack.Api;
using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Deliveries.Commands.AssignDelivery;
using LogiTrack.Application.Deliveries.Commands.CreateDelivery;
using LogiTrack.Application.Deliveries.Commands.UpdateDeliveryStatus;
using LogiTrack.Application.Deliveries.DTOs;
using LogiTrack.Application.Deliveries.Queries.GetDeliveryTracking;
using LogiTrack.Application.Deliveries.Queries.GetDeliveries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Api.Controllers.V2;

[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/deliveries")]
public class DeliveriesV2Controller : ControllerBase
{
    private readonly ISender _sender; private readonly IApplicationDbContext _context; private readonly IIdempotencyStore _idempotency;
    public DeliveriesV2Controller(ISender sender,IApplicationDbContext context,IIdempotencyStore idempotency)=>(_sender,_context,_idempotency)=(sender,context,idempotency);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]int page=1,[FromQuery]int pageSize=20,CancellationToken ct=default)
    {
        page=Math.Max(1,page); pageSize=Math.Clamp(pageSize,1,50); var q=_context.Deliveries.AsNoTracking(); var total=await q.CountAsync(ct);
        var rows=await q.OrderByDescending(x=>x.CreatedAt).Skip((page-1)*pageSize).Take(pageSize).Select(x=>new DeliveryDto(x.Id,x.TrackingNumber,x.CustomerId,x.DriverId,x.VehicleId,x.PickupAddress,x.DeliveryAddress,x.PackageDescription,x.WeightKg,x.Status,x.DeliveryFee,x.ExpectedDeliveryDate,x.DeliveredAt,x.CreatedAt)).ToListAsync(ct);
        var pages=(int)Math.Ceiling(total/(double)pageSize); return Ok(new {data=rows,meta=new {totalCount=total,page,pageSize,totalPages=pages,hasNext=page<pages,hasPrevious=page>1},links=new {self=$"/api/v2/deliveries?page={page}&pageSize={pageSize}",next=page<pages?$"/api/v2/deliveries?page={page+1}&pageSize={pageSize}":null,prev=page>1?$"/api/v2/deliveries?page={page-1}&pageSize={pageSize}":null,tracking="/api/v2/deliveries/{id}/tracking"}});
    }

    [HttpPost]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Create(CreateDeliveryCommand command,[FromHeader(Name="Idempotency-Key")]string? key,CancellationToken ct)
    {
        if(!string.IsNullOrWhiteSpace(key)&&_idempotency.TryGet("deliveries",key,out var cached))return StatusCode(cached.StatusCode,cached.Body);
        var result=await _sender.Send(command,ct); var response=new {data=result,links=new {self=$"/api/v2/deliveries/{result.Id}",tracking=$"/api/v2/deliveries/{result.Id}/tracking"}};
        if(!string.IsNullOrWhiteSpace(key))_idempotency.TryAdd("deliveries",key,new IdempotencyResult(201,response));
        return Created($"/api/v2/deliveries/{result.Id}",response);
    }

    [HttpPost("{id:int}/assign")][EnableRateLimiting("writes")]
    public async Task<IActionResult> Assign(int id,AssignDeliveryCommand command,CancellationToken ct){if(id!=command.DeliveryId)return BadRequest(new ProblemDetails{Title="Route ID mismatch",Detail="Route ID and command ID must match.",Status=400});var result=await _sender.Send(command,ct);return result is null?NotFound(new ProblemDetails{Title="Delivery not found",Status=404}):Ok(result);}
    [HttpPut("{id:int}/status")][EnableRateLimiting("writes")]
    public async Task<IActionResult> UpdateStatus(int id,UpdateDeliveryStatusCommand command,CancellationToken ct){if(id!=command.DeliveryId)return BadRequest(new ProblemDetails{Title="Route ID mismatch",Detail="Route ID and command ID must match.",Status=400});var result=await _sender.Send(command,ct);return result is null?NotFound(new ProblemDetails{Title="Delivery not found",Status=404}):Ok(result);}
    [HttpGet("{id:int}/tracking")]
    public async Task<IActionResult> GetTracking(int id,CancellationToken ct){var result=await _sender.Send(new GetDeliveryTrackingQuery(id),ct);return result is null?NotFound(new ProblemDetails{Title="Delivery not found",Status=404}):Ok(result);}
}
