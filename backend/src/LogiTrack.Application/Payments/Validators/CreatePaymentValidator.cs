using FluentValidation;
using LogiTrack.Application.Payments.Commands.CreatePayment;

namespace LogiTrack.Application.Payments.Validators;

public class CreatePaymentValidator
    : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.DeliveryId)
            .GreaterThan(0);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .MaximumLength(50);
    }
}