namespace LogiTrack.Application.Hubs;

public interface IDeliveryHubClient
{
    Task DeliveryUpdated(object delivery);
}
