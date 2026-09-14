using LogiTrack.Application.Deliveries.DTOs;
using MediatR;

namespace LogiTrack.Application.Deliveries.Queries.GetDeliveries;

public record GetDeliveriesQuery
    : IRequest<List<DeliveryDto>>;