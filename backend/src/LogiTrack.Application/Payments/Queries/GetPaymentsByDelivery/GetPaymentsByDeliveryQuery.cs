using LogiTrack.Application.Payments.DTOs;
using MediatR;

namespace LogiTrack.Application.Payments.Queries.GetPaymentsByDelivery;

public record GetPaymentsByDeliveryQuery(
    int DeliveryId
) : IRequest<List<PaymentDto>>;