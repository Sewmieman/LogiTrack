using Microsoft.AspNetCore.SignalR;

namespace LogiTrack.Api.Hubs;

public sealed class DeliveryHub : Hub
{
    public Task JoinDeliveryGroup(int deliveryId)
    {
        return Groups.AddToGroupAsync(
            Context.ConnectionId,
            GroupName(deliveryId));
    }

    public Task LeaveDeliveryGroup(int deliveryId)
    {
        return Groups.RemoveFromGroupAsync(
            Context.ConnectionId,
            GroupName(deliveryId));
    }

    public static string GroupName(int deliveryId)
        => $"delivery:{deliveryId}";
}
