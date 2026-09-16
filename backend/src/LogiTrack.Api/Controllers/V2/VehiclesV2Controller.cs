using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Vehicles.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using LogiTrack.Application.Vehicles.Commands.CreateVehicle;
using LogiTrack.Application.Vehicles.Commands.DeleteVehicle;
using LogiTrack.Application.Vehicles.Commands.UpdateVehicle;
using LogiTrack.Application.Vehicles.Queries.GetVehicleById;
using LogiTrack.Application.Vehicles.Queries.GetVehicles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogiTrack.Api.Controllers.V2;

[ApiController]
[Authorize]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/vehicles")]
public class VehiclesV2Controller : ControllerBase
{
    private readonly ISender _sender; private readonly IApplicationDbContext _context;
    public VehiclesV2Controller(ISender sender, IApplicationDbContext context) => (_sender,_context)=(sender,context);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page=1,[FromQuery] int pageSize=20,CancellationToken cancellationToken=default)
    {
        page=Math.Max(1,page); pageSize=Math.Clamp(pageSize,1,50); var q=_context.Vehicles.AsNoTracking(); var total=await q.CountAsync(cancellationToken);
        var rows=await q.OrderByDescending(x=>x.Id).Skip((page-1)*pageSize).Take(pageSize).Select(x=>new VehicleDto(x.Id,x.PlateNumber,x.Model,x.Type,x.Year,x.CapacityKg,x.Status)).ToListAsync(cancellationToken);
        var pages=(int)Math.Ceiling(total/(double)pageSize); return Ok(new {data=rows,meta=new {totalCount=total,page,pageSize,totalPages=pages,hasNext=page<pages,hasPrevious=page>1},links=new {self=$"/api/v2/vehicles?page={page}&pageSize={pageSize}",next=page<pages?$"/api/v2/vehicles?page={page+1}&pageSize={pageSize}":null,prev=page>1?$"/api/v2/vehicles?page={page-1}&pageSize={pageSize}":null}});
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetVehicleByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Create(CreateVehicleCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Update(int id, UpdateVehicleCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route ID and command ID must match.");

        var result = await _sender.Send(command, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    [EnableRateLimiting("writes")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteVehicleCommand(id), cancellationToken);
        return result ? NoContent() : NotFound();
    }
}
