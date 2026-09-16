using Asp.Versioning;
using LogiTrack.Application.Payments.Commands.CreatePayment;
using LogiTrack.Application.Payments.Commands.PayPayment;
using LogiTrack.Application.Payments.Queries.GetPaymentsByDelivery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePaymentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:int}/pay")]
    public async Task<IActionResult> Pay(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new PayPaymentCommand(id),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpGet("delivery/{deliveryId:int}")]
    public async Task<IActionResult> GetByDelivery(
        int deliveryId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetPaymentsByDeliveryQuery(deliveryId),
            cancellationToken);

        return Ok(result);
    }
}