using LogiTrack.Application.Drivers.DTOs;
using MediatR;

namespace LogiTrack.Application.Drivers.Commands.CreateDriver;

public record CreateDriverCommand(
    string FirstName,
    string LastName,
    string Phone,
    string LicenseNumber
) : IRequest<DriverDto>;