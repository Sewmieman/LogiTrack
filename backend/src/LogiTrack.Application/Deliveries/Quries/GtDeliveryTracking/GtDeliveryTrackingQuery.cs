using LogiTrack.Application.Deliveries.DTOs;
using MediatR;

namespace LogiTrack.Application.Deliveries.Queries.GetDeliveryTracking;

public record GetDeliveryTrackingQuery(int DeliveryId)
    : IRequest<List<DeliveryTrackingDto>>;