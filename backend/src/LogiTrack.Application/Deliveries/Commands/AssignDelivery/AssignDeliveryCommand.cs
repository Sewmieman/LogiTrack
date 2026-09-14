using LogiTrack.Application.Deliveries.DTOs;
using MediatR;

namespace LogiTrack.Application.Deliveries.Commands.AssignDelivery;

public record AssignDeliveryCommand(
    int DeliveryId,
    int DriverId,
    int VehicleId
) : IRequest<DeliveryDto?>;