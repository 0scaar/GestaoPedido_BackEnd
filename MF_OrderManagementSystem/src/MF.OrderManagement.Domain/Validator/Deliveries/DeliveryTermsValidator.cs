using FluentValidation;
using MF.OrderManagement.Domain.Entities.Deliveries;

namespace MF.OrderManagement.Domain.Validator.Deliveries;

public class DeliveryTermsValidator : AbstractValidator<DeliveryTerms>
{
    public DeliveryTermsValidator()
    {
        RuleFor(dt => dt.OrderId)
            .NotEmpty()
            .WithMessage("Id do pedido é obrigatório");

        RuleFor(dt => dt.EstimatedDeliveryDate)
            .NotEmpty()
            .WithMessage("Data estimada de entrega é obrigatória")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Data estimada de entrega deve ser no futuro");

        RuleFor(dt => dt.DeliveryDays)
            .GreaterThan(0)
            .WithMessage("Prazo em dias deve ser maior que zero");
            // .LessThanOrEqualTo(365)
            // .WithMessage("Prazo em dias não pode ser maior que 365 dias");

        // RuleFor(dt => dt.CreatedAt)
        //     .LessThanOrEqualTo(DateTime.UtcNow)
        //     .WithMessage("Data de criação não pode ser no futuro");
    }
}