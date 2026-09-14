using LogiTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }
    DbSet<Driver> Drivers { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<Delivery> Deliveries { get; }
    DbSet<Payment> Payments { get; }
    DbSet<DeliveryTrackingEvent> DeliveryTrackingEvents { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}