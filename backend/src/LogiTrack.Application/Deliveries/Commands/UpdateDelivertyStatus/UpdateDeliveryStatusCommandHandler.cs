using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Deliveries.DTOs;
using LogiTrack.Domain.Entities;
using LogiTrack.Application.Notifications;
using LogiTrack.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Deliveries.Commands.UpdateDeliveryStatus;

public class UpdateDeliveryStatusHandler
    : IRequestHandler<UpdateDeliveryStatusCommand, DeliveryDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IDeliveryNotificationService _notificationService;

    public UpdateDeliveryStatusHandler(
        IApplicationDbContext context,
        IDeliveryNotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<DeliveryDto?> Handle(
        UpdateDeliveryStatusCommand request,
        CancellationToken cancellationToken)
    {
        var delivery = await _context.Deliveries
            .FirstOrDefaultAsync(
                x => x.Id == request.DeliveryId,
                cancellationToken);

        if (delivery is null)
            return null;

        ValidateTransition(
            delivery.Status,
            request.Status);

        delivery.Status = request.Status;

        if (request.Status == DeliveryStatus.Delivered)
        {
            delivery.DeliveredAt = DateTime.UtcNow;

            if (delivery.DriverId.HasValue)
            {
                var driver = await _context.Drivers
                    .FirstOrDefaultAsync(
                        x => x.Id == delivery.DriverId,
                        cancellationToken);

                if (driver is not null)
                    driver.Status = DriverStatus.Available;
            }

            if (delivery.VehicleId.HasValue)
            {
                var vehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(
                        x => x.Id == delivery.VehicleId,
                        cancellationToken);

                if (vehicle is not null)
                    vehicle.Status = VehicleStatus.Available;
            }
        }

        var trackingEvent = new DeliveryTrackingEvent
        {
            DeliveryId = delivery.Id,
            Status = request.Status.ToString(),
            Location = request.Location,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        _context.DeliveryTrackingEvents.Add(trackingEvent);

        await _context.SaveChangesAsync(cancellationToken);

        var result = new DeliveryDto(
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

        await _notificationService.DeliveryUpdatedAsync(
            result,
            cancellationToken);

        return result;
    }

    private static void ValidateTransition(
        DeliveryStatus current,
        DeliveryStatus next)
    {
        var valid = current switch
        {
            DeliveryStatus.Pending =>
                next == DeliveryStatus.Assigned,

            DeliveryStatus.Assigned =>
                next == DeliveryStatus.PickedUp,

            DeliveryStatus.PickedUp =>
                next == DeliveryStatus.InTransit,

            DeliveryStatus.InTransit =>
                next == DeliveryStatus.Delivered,

            _ => false
        };

        if (!valid)
        {
            throw new InvalidOperationException(
                $"Invalid delivery status transition: {current} -> {next}");
        }
    }
}