using LogiTrack.Application.Vehicles.DTOs;
using MediatR;

namespace LogiTrack.Application.Vehicles.Queries.GetVehicleById;

public record GetVehicleByIdQuery(int Id)
    : IRequest<VehicleDto?>;