using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Payments.DTOs;
using LogiTrack.Domain.Entities;
using LogiTrack.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Payments.Commands.CreatePayment;

public class CreatePaymentHandler
    : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly IApplicationDbContext _context;

    public CreatePaymentHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentDto> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var delivery = await _context.Deliveries
            .FirstOrDefaultAsync(
                x => x.Id == request.DeliveryId,
                cancellationToken);

        if (delivery is null)
            throw new KeyNotFoundException("Delivery not found.");

        if (request.Amount <= 0)
            throw new ArgumentException(
                "Payment amount must be greater than zero.");

        var payment = new Payment
        {
            DeliveryId = request.DeliveryId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

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