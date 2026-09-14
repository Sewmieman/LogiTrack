using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Drivers.DTOs;
using LogiTrack.Domain.Entities;
using MediatR;

namespace LogiTrack.Application.Drivers.Commands.CreateDriver;

public class CreateDriverHandler
    : IRequestHandler<CreateDriverCommand, DriverDto>
{
    private readonly IApplicationDbContext _context;

    public CreateDriverHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DriverDto> Handle(
        CreateDriverCommand request,
        CancellationToken cancellationToken)
    {
        var driver = new Driver
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            LicenseNumber = request.LicenseNumber,
            Status = Domain.Enums.DriverStatus.Available
        };

        _context.Drivers.Add(driver);

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