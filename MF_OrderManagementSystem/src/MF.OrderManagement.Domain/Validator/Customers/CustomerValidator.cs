using FluentValidation;
using MF.OrderManagement.Domain.Entities.Customers;

namespace MF.OrderManagement.Domain.Validator.Customers;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Nome do cliente é obrigatório")
            .MaximumLength(150)
            .WithMessage("Nome do cliente não pode exceder 150 caracteres");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email deve ser um endereço válido")
            .MaximumLength(150)
            .WithMessage("Email não pode exceder 150 caracteres");
    }
}