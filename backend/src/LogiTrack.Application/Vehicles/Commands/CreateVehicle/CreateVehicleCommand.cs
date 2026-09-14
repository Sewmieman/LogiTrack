using LogiTrack.Application.Vehicles.DTOs;
using MediatR;

namespace LogiTrack.Application.Vehicles.Commands.CreateVehicle;

public record CreateVehicleCommand(
    string PlateNumber,
    string Model,
    string Type,
    int Year,
    decimal CapacityKg
) : IRequest<VehicleDto>;