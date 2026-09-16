using LogiTrack.Application.Payments.DTOs;
using MediatR;

namespace LogiTrack.Application.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(
    int DeliveryId,
    decimal Amount,
    string PaymentMethod
) : IRequest<PaymentDto>;