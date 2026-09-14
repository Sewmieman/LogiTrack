namespace LogiTrack.Domain.Entities;

public class DeliveryTrackingEvent
{
    public int Id { get; set; }

    public int DeliveryId { get; set; }

    public Delivery Delivery { get; set; } = null!;

    public string Status { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}