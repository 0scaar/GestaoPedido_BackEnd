using FluentValidation;
using MF.OrderManagement.Domain.Entities.Orders;

namespace MF.OrderManagement.Domain.Validator.Orders;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(o => o.CustomerId)
            .NotEmpty()
            .WithMessage("Id do cliente é obrigatório");

        RuleFor(o => o.PaymentConditionId)
            .NotEmpty()
            .WithMessage("Id da condição de pagamento é obrigatório");

        RuleFor(o => o.OrderDate)
            .NotEmpty()
            .WithMessage("Data do pedido é obrigatória")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Data do pedido não pode ser no futuro");

        RuleFor(o => o.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Valor total não pode ser negativo");

        RuleFor(o => o.Status)
            .IsInEnum()
            .WithMessage("Status do pedido inválido");

        // RuleFor(o => o.CreatedAt)
        //     .LessThanOrEqualTo(DateTime.UtcNow)
        //     .WithMessage("Data de criação não pode ser no futuro");

        RuleFor(o => o.Items)
            .NotEmpty()
            .WithMessage("Pedido deve ter pelo menos um item");
    }
}