using LogiTrack.Application.Vehicles.DTOs;
using LogiTrack.Domain.Enums;
using MediatR;

namespace LogiTrack.Application.Vehicles.Commands.UpdateVehicle;

public record UpdateVehicleCommand(
    int Id,
    string PlateNumber,
    string Model,
    string Type,
    int Year,
    decimal CapacityKg,
    VehicleStatus Status
) : IRequest<VehicleDto?>;