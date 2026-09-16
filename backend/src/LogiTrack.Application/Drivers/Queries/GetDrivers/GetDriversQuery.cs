using LogiTrack.Application.Drivers.DTOs;
using MediatR;

namespace LogiTrack.Application.Drivers.Queries.GetDrivers;

public record GetDriversQuery : IRequest<List<DriverDto>>;