using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Payments.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using LogiTrack.Application.Payments.Commands.CreatePayment;
using LogiTrack.Application.Payments.Commands.PayPayment;
using LogiTrack.Application.Payments.Queries.GetPaymentsByDelivery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers.V2;

[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/payments")]
public class PaymentsV2Controller : ControllerBase
{
    private readonly ISender _sender; private readonly IApplicationDbContext _context;
    public PaymentsV2Controller(ISender sender, IApplicationDbContext context) => (_sender,_context)=(sender,context);

    [HttpPost]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Create(CreatePaymentCommand command, CancellationToken cancellationToken) =>
        Ok(await _sender.Send(command, cancellationToken));

    [HttpPut("{id:int}/pay")]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Pay(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new PayPaymentCommand(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("delivery/{deliveryId:int}")]
    public async Task<IActionResult> GetByDelivery(int deliveryId,[FromQuery]int page=1,[FromQuery]int pageSize=20,CancellationToken cancellationToken=default)
    {
        page=Math.Max(1,page); pageSize=Math.Clamp(pageSize,1,50); var q=_context.Payments.AsNoTracking().Where(x=>x.DeliveryId==deliveryId); var total=await q.CountAsync(cancellationToken);
        var rows=await q.OrderByDescending(x=>x.CreatedAt).Skip((page-1)*pageSize).Take(pageSize).Select(x=>new PaymentDto(x.Id,x.DeliveryId,x.Amount,x.PaymentMethod,x.Status,x.PaidAt,x.CreatedAt)).ToListAsync(cancellationToken);
        var pages=(int)Math.Ceiling(total/(double)pageSize); return Ok(new {data=rows,meta=new {totalCount=total,page,pageSize,totalPages=pages,hasNext=page<pages,hasPrevious=page>1},links=new {self=$"/api/v2/payments/delivery/{deliveryId}?page={page}&pageSize={pageSize}",next=page<pages?$"/api/v2/payments/delivery/{deliveryId}?page={page+1}&pageSize={pageSize}":null,prev=page>1?$"/api/v2/payments/delivery/{deliveryId}?page={page-1}&pageSize={pageSize}":null}});
    }
}
