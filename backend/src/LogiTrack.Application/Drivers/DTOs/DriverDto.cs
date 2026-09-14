using LogiTrack.Domain.Enums;

namespace LogiTrack.Application.Drivers.DTOs;

public record DriverDto(
    int Id,
    string FirstName,
    string LastName,
    string Phone,
    string LicenseNumber,
    DriverStatus Status
);