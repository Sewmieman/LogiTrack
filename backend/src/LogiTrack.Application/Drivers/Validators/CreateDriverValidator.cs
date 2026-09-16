using FluentValidation;
using LogiTrack.Application.Drivers.Commands.CreateDriver;

namespace LogiTrack.Application.Drivers.Validators;

public class CreateDriverValidator
    : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverValidator()
    {
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
    }
}