namespace LogiTrack.Application.Deliveries.DTOs;

public record DeliveryTrackingDto(
    int Id,
    int DeliveryId,
    string Status,
    string Location,
    string? Notes,
    DateTime CreatedAt
);