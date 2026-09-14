using LogiTrack.Domain.Enums;

namespace LogiTrack.Application.Vehicles.DTOs;

public record VehicleDto(
    int Id,
    string PlateNumber,
    string Model,
    string Type,
    int Year,
    decimal CapacityKg,
    VehicleStatus Status
);