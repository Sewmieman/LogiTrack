using LogiTrack.Application.Common.Interfaces;
using LogiTrack.Application.Deliveries.DTOs;
using LogiTrack.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiTrack.Application.Deliveries.Commands.CreateDelivery;

public class CreateDeliveryHandler
    : IRequestHandler<CreateDeliveryCommand, DeliveryDto>
{
    private readonly IApplicationDbContext _context;

    public CreateDeliveryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryDto> Handle(
        CreateDeliveryCommand request,
        CancellationToken cancellationToken)
    {
        var customerExists = await _context.Customers
            .AnyAsync(
                x => x.Id == request.CustomerId,
                cancellationToken);

        if (!customerExists)
            throw new KeyNotFoundException("Customer not found.");

        var delivery = new Delivery
        {
            TrackingNumber = GenerateTrackingNumber(),
            CustomerId = request.CustomerId,
            PickupAddress = request.PickupAddress,
            DeliveryAddress = request.DeliveryAddress,
            PackageDescription = request.PackageDescription,
            WeightKg = request.WeightKg,
            DeliveryFee = request.DeliveryFee,
            ExpectedDeliveryDate = request.ExpectedDeliveryDate,
            Status = Domain.Enums.DeliveryStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Deliveries.Add(delivery);

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

    private static string GenerateTrackingNumber()
    {
        return $"LT-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }
}