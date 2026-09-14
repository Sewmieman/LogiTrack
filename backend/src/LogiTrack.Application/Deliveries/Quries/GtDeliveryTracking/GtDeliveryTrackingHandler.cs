using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Deliveries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Deliveries.Queries.GetDeliveryTracking;

public class GetDeliveryTrackingHandler
    : IRequestHandler<
        GetDeliveryTrackingQuery,
        List<DeliveryTrackingDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDeliveryTrackingHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeliveryTrackingDto>> Handle(
        GetDeliveryTrackingQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.DeliveryTrackingEvents
            .AsNoTracking()
            .Where(x => x.DeliveryId == request.DeliveryId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DeliveryTrackingDto(
                x.Id,
                x.DeliveryId,
                x.Status,
                x.Location,
                x.Notes,
                x.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}