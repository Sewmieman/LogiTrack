using LogiTrack.Domain.Enums;

namespace LogiTrack.Domain.Entities;

public class Delivery
{
    public int Id { get; set; }

    public string TrackingNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public int? DriverId { get; set; }

    public Driver? Driver { get; set; }

    public int? VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    public string PickupAddress { get; set; } = string.Empty;

    public string DeliveryAddress { get; set; } = string.Empty;

    public string PackageDescription { get; set; } = string.Empty;

    public decimal WeightKg { get; set; }

    public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;

    public decimal DeliveryFee { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public ICollection<DeliveryTrackingEvent> TrackingEvents { get; set; }
        = new List<DeliveryTrackingEvent>();
}