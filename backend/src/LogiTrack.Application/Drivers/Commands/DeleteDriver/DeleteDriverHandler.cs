using LogiTrack.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Drivers.Commands.DeleteDriver;

public class DeleteDriverHandler
    : IRequestHandler<DeleteDriverCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteDriverHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeleteDriverCommand request,
        CancellationToken cancellationToken)
    {
        var driver = await _context.Drivers
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (driver is null)
            return false;

        _context.Drivers.Remove(driver);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}