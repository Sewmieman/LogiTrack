using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Payments.DTOs;
using LogiTrack.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Payments.Commands.PayPayment;

public class PayPaymentHandler
    : IRequestHandler<PayPaymentCommand, PaymentDto?>
{
    private readonly IApplicationDbContext _context;

    public PayPaymentHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentDto?> Handle(
        PayPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (payment is null)
            return null;

        if (payment.Status == PaymentStatus.Paid)
            throw new InvalidOperationException(
                "Payment has already been paid.");

        payment.Status = PaymentStatus.Paid;
        payment.PaidAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new PaymentDto(
            payment.Id,
            payment.DeliveryId,
            payment.Amount,
            payment.PaymentMethod,
            payment.Status,
            payment.PaidAt,
            payment.CreatedAt
        );
    }
}