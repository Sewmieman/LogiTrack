using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Payments.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Payments.Queries.GetPaymentsByDelivery;

public class GetPaymentsByDeliveryHandler
    : IRequestHandler<
        GetPaymentsByDeliveryQuery,
        List<PaymentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPaymentsByDeliveryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentDto>> Handle(
        GetPaymentsByDeliveryQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(x => x.DeliveryId == request.DeliveryId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new PaymentDto(
                x.Id,
                x.DeliveryId,
                x.Amount,
                x.PaymentMethod,
                x.Status,
                x.PaidAt,
                x.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}