using FluentValidation;
using MF.OrderManagement.Application.Orders.DTOs;

namespace MF.OrderManagement.Application.Orders.Validators;

public sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Customer.Name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Customer.Email is required.")
            .MaximumLength(100)
            .EmailAddress().WithMessage("Customer.Email is invalid.");
    }
}