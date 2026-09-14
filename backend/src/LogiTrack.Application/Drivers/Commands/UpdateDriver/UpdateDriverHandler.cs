using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Drivers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Drivers.Commands.UpdateDriver;

public class UpdateDriverHandler
    : IRequestHandler<UpdateDriverCommand, DriverDto?>
{
    private readonly IApplicationDbContext _context;

    public UpdateDriverHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DriverDto?> Handle(
        UpdateDriverCommand request,
        CancellationToken cancellationToken)
    {
        var driver = await _context.Drivers
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (driver is null)
            return null;

        driver.FirstName = request.FirstName;
        driver.LastName = request.LastName;
        driver.Phone = request.Phone;
        driver.LicenseNumber = request.LicenseNumber;
        driver.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        return new DriverDto(
            driver.Id,
            driver.FirstName,
            driver.LastName,
            driver.Phone,
            driver.LicenseNumber,
            driver.Status
        );
    }
}