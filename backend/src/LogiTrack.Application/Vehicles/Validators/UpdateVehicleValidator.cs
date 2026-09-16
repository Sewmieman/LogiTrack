using FluentValidation;
using LogiTrack.Application.Vehicles.Commands.UpdateVehicle;

namespace LogiTrack.Application.Vehicles.Validators;

public class UpdateVehicleValidator
    : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

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

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}