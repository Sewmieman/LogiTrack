using LogiTrack.Application.Deliveries.DTOs;
using LogiTrack.Domain.Enums;
using MediatR;

namespace LogiTrack.Application.Deliveries.Commands.UpdateDeliveryStatus;

public record UpdateDeliveryStatusCommand(
    int DeliveryId,
    DeliveryStatus Status,
    string Location,
    string? Notes
) : IRequest<DeliveryDto?>;