using FluentValidation;
using LogiTrack.Application.Drivers.Commands.UpdateDriver;

namespace LogiTrack.Application.Drivers.Validators;

public class UpdateDriverValidator
    : AbstractValidator<UpdateDriverCommand>
{
    public UpdateDriverValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.LicenseNumber)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}