using LogiTrack.Application.Deliveries.DTOs;
using MediatR;

namespace LogiTrack.Application.Deliveries.Commands.CreateDelivery;

public record CreateDeliveryCommand(
    int CustomerId,
    string PickupAddress,
    string DeliveryAddress,
    string PackageDescription,
    decimal WeightKg,
    decimal DeliveryFee,
    DateTime? ExpectedDeliveryDate
) : IRequest<DeliveryDto>;