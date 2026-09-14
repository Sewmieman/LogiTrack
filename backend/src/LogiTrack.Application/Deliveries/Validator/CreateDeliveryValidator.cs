using FluentValidation;
using LogiTrack.Application.Deliveries.Commands.CreateDelivery;

namespace LogiTrack.Application.Deliveries.Validators;

public class CreateDeliveryValidator
    : AbstractValidator<CreateDeliveryCommand>
{
    public CreateDeliveryValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0);

        RuleFor(x => x.PickupAddress)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.DeliveryAddress)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.PackageDescription)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.WeightKg)
            .GreaterThan(0);

        RuleFor(x => x.DeliveryFee)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ExpectedDeliveryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpectedDeliveryDate.HasValue);
    }
}