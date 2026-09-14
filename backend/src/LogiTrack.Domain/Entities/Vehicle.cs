using LogiTrack.Domain.Enums;

namespace LogiTrack.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    public string PlateNumber { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal CapacityKg { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Available;

    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}