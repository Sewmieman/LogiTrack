using LogiTrack.Application.Drivers.DTOs;
using LogiTrack.Domain.Enums;
using MediatR;

namespace LogiTrack.Application.Drivers.Commands.UpdateDriver;

public record UpdateDriverCommand(
    int Id,
    string FirstName,
    string LastName,
    string Phone,
    string LicenseNumber,
    DriverStatus Status
) : IRequest<DriverDto?>;