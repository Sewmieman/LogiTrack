using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Vehicles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Vehicles.Queries.GetVehicles;

public class GetVehiclesHandler
    : IRequestHandler<GetVehiclesQuery, List<VehicleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVehiclesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VehicleDto>> Handle(
        GetVehiclesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => new VehicleDto(
                x.Id,
                x.PlateNumber,
                x.Model,
                x.Type,
                x.Year,
                x.CapacityKg,
                x.Status
            ))
            .ToListAsync(cancellationToken);
    }
}