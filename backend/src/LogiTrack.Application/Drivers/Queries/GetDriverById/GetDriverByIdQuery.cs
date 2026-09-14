using LogiTrack.Application.Drivers.DTOs;
using MediatR;

namespace LogiTrack.Application.Drivers.Queries.GetDriverById;

public record GetDriverByIdQuery(int Id)
    : IRequest<DriverDto?>;