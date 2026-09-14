using LogiTrack.Domain.Enums;

namespace LogiTrack.Domain.Entities;

public class Payment
{
    public int Id { get; set; }

    public int DeliveryId { get; set; }

    public Delivery Delivery { get; set; } = null!;

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}