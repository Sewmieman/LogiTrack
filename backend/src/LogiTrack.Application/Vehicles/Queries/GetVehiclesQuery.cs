using LogiTrack.Application.Vehicles.DTOs;
using MediatR;

namespace LogiTrack.Application.Vehicles.Queries.GetVehicles;

public record GetVehiclesQuery : IRequest<List<VehicleDto>>;