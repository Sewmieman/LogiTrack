using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Drivers.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Drivers.Queries.GetDriverById;

public class GetDriverByIdHandler
    : IRequestHandler<GetDriverByIdQuery, DriverDto?>
{
    private readonly IApplicationDbContext _context;

    public GetDriverByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DriverDto?> Handle(
        GetDriverByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Drivers
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new DriverDto(
                x.Id,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.LicenseNumber,
                x.Status
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}