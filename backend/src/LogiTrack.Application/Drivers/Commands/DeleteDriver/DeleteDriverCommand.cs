using MediatR;

namespace LogiTrack.Application.Drivers.Commands.DeleteDriver;

public record DeleteDriverCommand(int Id)
    : IRequest<bool>;