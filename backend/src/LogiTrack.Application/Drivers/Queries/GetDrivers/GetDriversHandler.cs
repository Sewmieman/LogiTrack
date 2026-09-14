using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Drivers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Drivers.Queries.GetDrivers;

public class GetDriversHandler
    : IRequestHandler<GetDriversQuery, List<DriverDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDriversHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DriverDto>> Handle(
        GetDriversQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Drivers
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => new DriverDto(
                x.Id,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.LicenseNumber,
                x.Status
            ))
            .ToListAsync(cancellationToken);
    }
}