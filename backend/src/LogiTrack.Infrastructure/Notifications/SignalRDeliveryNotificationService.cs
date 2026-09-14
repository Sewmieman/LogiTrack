using LogiTrack.Application.Deliveries.DTOs;
using LogiTrack.Application.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace LogiTrack.Infrastructure.Notifications;

public sealed class SignalRDeliveryNotificationService : IDeliveryNotificationService
{
    private readonly IHubContext<DeliveryHub> _hub;

    public SignalRDeliveryNotificationService(IHubContext<DeliveryHub> hub)
    {
        _hub = hub;
    }

    public Task DeliveryUpdatedAsync(
        DeliveryDto delivery,
        CancellationToken cancellationToken = default)
    {
        return _hub.Clients
            .Group(DeliveryHub.GroupName(delivery.Id))
            .SendAsync("deliveryUpdated", delivery, cancellationToken);
    }
}
