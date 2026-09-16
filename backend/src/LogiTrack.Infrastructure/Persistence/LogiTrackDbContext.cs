using LogiTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LogiTrack.Application.Common.Interfaces;
namespace LogiTrack.Infrastructure.Persistence;

using LogiTrack.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
public class LogiTrackDbContext
    : IdentityDbContext<LogiTrackUser>, IApplicationDbContext
{
    public LogiTrackDbContext(
        DbContextOptions<LogiTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<DeliveryTrackingEvent> DeliveryTrackingEvents =>
        Set<DeliveryTrackingEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Phone)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(200);

            entity.Property(x => x.Address)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Phone)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.LicenseNumber)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.LicenseNumber)
                .IsUnique();
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.Property(x => x.PlateNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.CapacityKg)
                .HasPrecision(10, 2);

            entity.HasIndex(x => x.PlateNumber)
                .IsUnique();
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.Property(x => x.TrackingNumber)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(x => x.TrackingNumber)
                .IsUnique();

            entity.Property(x => x.WeightKg)
                .HasPrecision(10, 2);

            entity.Property(x => x.DeliveryFee)
                .HasPrecision(12, 2);

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Driver)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.DriverId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Vehicle)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(x => x.Amount)
                .HasPrecision(12, 2);

            entity.Property(x => x.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<DeliveryTrackingEvent>(entity =>
        {
            entity.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Location)
                .HasMaxLength(300);
        });
    }
}