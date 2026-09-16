using LogiTrack.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleHandler
    : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteVehicleHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeleteVehicleCommand request,
        CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (vehicle is null)
            return false;

        _context.Vehicles.Remove(vehicle);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}