using LogiTrack.Application.Payments.DTOs;
using MediatR;

namespace LogiTrack.Application.Payments.Commands.PayPayment;

public record PayPaymentCommand(
    int Id
) : IRequest<PaymentDto?>;