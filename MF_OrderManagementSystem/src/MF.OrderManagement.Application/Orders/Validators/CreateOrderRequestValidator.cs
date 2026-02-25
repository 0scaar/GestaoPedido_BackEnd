using FluentValidation;
using MF.OrderManagement.Application.Orders.DTOs;

namespace MF.OrderManagement.Application.Orders.Validators;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Customer)
            .NotNull().WithMessage("Customer is required.")
            .SetValidator(new CreateCustomerRequestValidator()!);

        RuleFor(x => x.PaymentCondition)
            .NotNull().WithMessage("PaymentCondition is required.")
            .SetValidator(new CreatePaymentConditionRequestValidator()!);

        RuleFor(x => x.Items)
            .NotNull().WithMessage("Items is required.")
            .Must(x => x is { Count: > 0 }).WithMessage("Order must have at least one item.");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateOrderItemRequestValidator());
    }
}