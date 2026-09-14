using LogiTrack.Domain.Enums;

namespace LogiTrack.Application.Deliveries.DTOs;

public record DeliveryDto(
    int Id,
    string TrackingNumber,
    int CustomerId,
    int? DriverId,
    int? VehicleId,
    string PickupAddress,
    string DeliveryAddress,
    string PackageDescription,
    decimal WeightKg,
    DeliveryStatus Status,
    decimal DeliveryFee,
    DateTime? ExpectedDeliveryDate,
    DateTime? DeliveredAt,
    DateTime CreatedAt
);