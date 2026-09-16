using MediatR;

namespace LogiTrack.Application.Vehicles.Commands.DeleteVehicle;

public record DeleteVehicleCommand(int Id) : IRequest<bool>;