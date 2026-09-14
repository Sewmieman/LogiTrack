using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Vehicles.DTOs;
using LogiTrack.Domain.Entities;
using LogiTrack.Domain.Enums;
using MediatR;

namespace LogiTrack.Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleHandler
    : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IApplicationDbContext _context;

    public CreateVehicleHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleDto> Handle(
        CreateVehicleCommand request,
        CancellationToken cancellationToken)
    {
        var vehicle = new Vehicle
        {
            PlateNumber = request.PlateNumber,
            Model = request.Model,
            Type = request.Type,
            Year = request.Year,
            CapacityKg = request.CapacityKg,
            Status = VehicleStatus.Available
        };

        _context.Vehicles.Add(vehicle);

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