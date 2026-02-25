using FluentValidation;
using MF.OrderManagement.Domain.Entities.Orders;

namespace MF.OrderManagement.Domain.Validator.Orders;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(oi => oi.OrderId)
            .NotEmpty()
            .WithMessage("Id do pedido é obrigatório");

        RuleFor(oi => oi.ProductName)
            .NotEmpty()
            .WithMessage("Nome do produto é obrigatório")
            .MaximumLength(150)
            .WithMessage("Nome do produto não pode exceder 150 caracteres");

        RuleFor(oi => oi.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantidade deve ser maior que zero");

        RuleFor(oi => oi.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Preço unitário deve ser maior que zero");

        RuleFor(oi => oi.TotalPrice)
            .GreaterThan(0)
            .WithMessage("Preço total deve ser maior que zero")
            .Equal(oi => oi.Quantity * oi.UnitPrice)
            .WithMessage("Preço total deve ser igual a quantidade multiplicada pelo preço unitário");
    }
}