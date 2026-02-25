using FluentValidation;
using MF.OrderManagement.Application.Orders.DTOs;

namespace MF.OrderManagement.Application.Orders.Validators;

public sealed class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
{
    public CreateOrderItemRequestValidator()
    {
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Items[].ProductName is required.")
            .MaximumLength(150);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Items[].Quantity must be > 0.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Items[].UnitPrice must be > 0.");
    }
}