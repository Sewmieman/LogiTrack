using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Deliveries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Deliveries.Queries.GetDeliveries;

public class GetDeliveriesHandler
    : IRequestHandler<GetDeliveriesQuery, List<DeliveryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDeliveriesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeliveryDto>> Handle(
        GetDeliveriesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Deliveries
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DeliveryDto(
                x.Id,
                x.TrackingNumber,
                x.CustomerId,
                x.DriverId,
                x.VehicleId,
                x.PickupAddress,
                x.DeliveryAddress,
                x.PackageDescription,
                x.WeightKg,
                x.Status,
                x.DeliveryFee,
                x.ExpectedDeliveryDate,
                x.DeliveredAt,
                x.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}