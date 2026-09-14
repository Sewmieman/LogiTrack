using Asp.Versioning;
using LogiTrack.Application.Drivers.Commands.CreateDriver;
using LogiTrack.Application.Drivers.Commands.DeleteDriver;
using LogiTrack.Application.Drivers.Commands.UpdateDriver;
using LogiTrack.Application.Drivers.Queries.GetDriverById;
using LogiTrack.Application.Drivers.Queries.GetDrivers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
[Authorize]
[Route("api/v{version:apiVersion}/drivers")]
public class DriversController : ControllerBase
{
    private readonly ISender _sender;

    public DriversController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetDriversQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetDriverByIdQuery(id),
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDriverCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                version = "1",
                id = result.Id
            },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateDriverCommand command,
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

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(
            new DeleteDriverCommand(id),
            cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}