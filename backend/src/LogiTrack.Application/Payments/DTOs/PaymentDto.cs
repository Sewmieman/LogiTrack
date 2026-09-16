using LogiTrack.Domain.Enums;

namespace LogiTrack.Application.Payments.DTOs;

public record PaymentDto(
    int Id,
    int DeliveryId,
    decimal Amount,
    string PaymentMethod,
    PaymentStatus Status,
    DateTime? PaidAt,
    DateTime CreatedAt
);