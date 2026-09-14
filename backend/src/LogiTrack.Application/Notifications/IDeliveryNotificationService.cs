using LogiTrack.Application.Deliveries.DTOs;

namespace LogiTrack.Application.Notifications;

public interface IDeliveryNotificationService
{
    Task DeliveryUpdatedAsync(
        DeliveryDto delivery,
        CancellationToken cancellationToken = default);
}
