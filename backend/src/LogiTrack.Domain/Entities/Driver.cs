using LogiTrack.Domain.Enums;

namespace LogiTrack.Domain.Entities;

public class Driver
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public DriverStatus Status { get; set; } = DriverStatus.Available;

    public ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}