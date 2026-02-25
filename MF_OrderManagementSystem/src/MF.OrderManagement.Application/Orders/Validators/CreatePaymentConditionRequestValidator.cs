using FluentValidation;
using MF.OrderManagement.Application.Orders.DTOs;

namespace MF.OrderManagement.Application.Orders.Validators;

public sealed class CreatePaymentConditionRequestValidator : AbstractValidator<CreatePaymentConditionRequest>
{
    public CreatePaymentConditionRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("PaymentCondition.Description is required.")
            .MaximumLength(100);

        RuleFor(x => x.NumberOfInstallments)
            .GreaterThan(0).WithMessage("PaymentCondition.NumberOfInstallments must be > 0.");
    }
}