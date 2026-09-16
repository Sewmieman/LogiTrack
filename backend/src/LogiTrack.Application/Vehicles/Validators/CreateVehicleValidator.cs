using FluentValidation;
using LogiTrack.Application.Vehicles.Commands.CreateVehicle;

namespace LogiTrack.Application.Vehicles.Validators;

public class CreateVehicleValidator
    : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.PlateNumber)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Type)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Year)
            .InclusiveBetween(1990, DateTime.UtcNow.Year + 1);

        RuleFor(x => x.CapacityKg)
            .GreaterThan(0);
    }
}