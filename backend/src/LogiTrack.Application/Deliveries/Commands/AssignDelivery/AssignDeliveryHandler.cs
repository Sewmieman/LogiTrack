using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Deliveries.DTOs;
using LogiTrack.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Deliveries.Commands.AssignDelivery;

public class AssignDeliveryHandler
    : IRequestHandler<AssignDeliveryCommand, DeliveryDto?>
{
    private readonly IApplicationDbContext _context;

    public AssignDeliveryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryDto?> Handle(
        AssignDeliveryCommand request,
        CancellationToken cancellationToken)
    {
        var delivery = await _context.Deliveries
            .FirstOrDefaultAsync(
                x => x.Id == request.DeliveryId,
                cancellationToken);

        if (delivery is null)
            return null;

        var driver = await _context.Drivers
            .FirstOrDefaultAsync(
                x => x.Id == request.DriverId,
                cancellationToken);

        if (driver is null)
            throw new KeyNotFoundException("Driver not found.");

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(
                x => x.Id == request.VehicleId,
                cancellationToken);

        if (vehicle is null)
            throw new KeyNotFoundException("Vehicle not found.");

        if (driver.Status != DriverStatus.Available)
            throw new InvalidOperationException(
                "Driver is not available.");

        if (vehicle.Status != VehicleStatus.Available)
            throw new InvalidOperationException(
                "Vehicle is not available.");

        if (vehicle.CapacityKg < delivery.WeightKg)
            throw new InvalidOperationException(
                "Vehicle capacity is insufficient.");

        delivery.DriverId = driver.Id;
        delivery.VehicleId = vehicle.Id;
        delivery.Status = DeliveryStatus.Assigned;

        driver.Status = DriverStatus.Busy;
        vehicle.Status = VehicleStatus.InUse;

        await _context.SaveChangesAsync(cancellationToken);

        return new DeliveryDto(
            delivery.Id,
            delivery.TrackingNumber,
            delivery.CustomerId,
            delivery.DriverId,
            delivery.VehicleId,
            delivery.PickupAddress,
            delivery.DeliveryAddress,
            delivery.PackageDescription,
            delivery.WeightKg,
            delivery.Status,
            delivery.DeliveryFee,
            delivery.ExpectedDeliveryDate,
            delivery.DeliveredAt,
            delivery.CreatedAt
        );
    }
}