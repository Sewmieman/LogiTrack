using Asp.Versioning;
using LogiTrack.Application.Vehicles.Commands.CreateVehicle;
using LogiTrack.Application.Vehicles.Commands.DeleteVehicle;
using LogiTrack.Application.Vehicles.Commands.UpdateVehicle;
using LogiTrack.Application.Vehicles.Queries.GetVehicleById;
using LogiTrack.Application.Vehicles.Queries.GetVehicles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers.V2;

[ApiController]
[ApiVersion(2.0)]
[Authorize]
[Route("api/v{version:apiVersion}/vehicles")]
public class VehiclesController : ControllerBase
{
    private readonly ISender _sender;

    public VehiclesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        return Ok(await _sender.Send(
            new GetVehiclesQuery(),
            cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetVehicleByIdQuery(id),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                version = "2",
                id = result.Id
            },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID and command ID must match.");
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeleteVehicleCommand(id),
            cancellationToken);

        return result
            ? NoContent()
            : NotFound();
    }
}