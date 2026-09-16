using Microsoft.AspNetCore.Authorization;
using LogiTrack.Application.Deliveries.Commands.AssignDelivery;
using LogiTrack.Application.Deliveries.Commands.CreateDelivery;
using LogiTrack.Application.Deliveries.Commands.UpdateDeliveryStatus;
using LogiTrack.Application.Deliveries.Queries.GetDeliveryTracking;
using LogiTrack.Application.Deliveries.Queries.GetDeliveries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/deliveries")]
public class DeliveriesController : ControllerBase
{
    private readonly ISender _sender;

    public DeliveriesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetDeliveriesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDeliveryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> Assign(
        int id,
        AssignDeliveryCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.DeliveryId)
            return BadRequest(
                "Route ID and command ID must match.");

        var result = await _sender.Send(
            command,
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateDeliveryStatusCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.DeliveryId)
            return BadRequest(
                "Route ID and command ID must match.");

        var result = await _sender.Send(
            command,
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpGet("{id:int}/tracking")]
    public async Task<IActionResult> GetTracking(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetDeliveryTrackingQuery(id),
            cancellationToken);

        return Ok(result);
    }
}