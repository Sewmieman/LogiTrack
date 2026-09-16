using Microsoft.AspNetCore.SignalR;

namespace LogiTrack.Infrastructure.Notifications;

public sealed class DeliveryHub : Hub
{
    public static string GroupName(int deliveryId)
        => $"delivery:{deliveryId}";

    public Task JoinDeliveryGroup(int deliveryId)
        => Groups.AddToGroupAsync(Context.ConnectionId, GroupName(deliveryId));

    public Task LeaveDeliveryGroup(int deliveryId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(deliveryId));
}
