using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Vehicles.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleHandler
    : IRequestHandler<UpdateVehicleCommand, VehicleDto?>
{
    private readonly IApplicationDbContext _context;

    public UpdateVehicleHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleDto?> Handle(
        UpdateVehicleCommand request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (vehicle is null)
            return null;

        vehicle.PlateNumber = request.PlateNumber;
        vehicle.Model = request.Model;
        vehicle.Type = request.Type;
        vehicle.Year = request.Year;
        vehicle.CapacityKg = request.CapacityKg;
        vehicle.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return new VehicleDto(
            vehicle.Id,
            vehicle.PlateNumber,
            vehicle.Model,
            vehicle.Type,
            vehicle.Year,
            vehicle.CapacityKg,
            vehicle.Status
        );
    }
}