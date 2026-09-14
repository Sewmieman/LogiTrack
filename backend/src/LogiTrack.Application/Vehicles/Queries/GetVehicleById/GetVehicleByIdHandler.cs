using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Vehicles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Vehicles.Queries.GetVehicleById;

public class GetVehicleByIdHandler
    : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    private readonly IApplicationDbContext _context;

    public GetVehicleByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleDto?> Handle(
        GetVehicleByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new VehicleDto(
                x.Id,
                x.PlateNumber,
                x.Model,
                x.Type,
                x.Year,
                x.CapacityKg,
                x.Status
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}