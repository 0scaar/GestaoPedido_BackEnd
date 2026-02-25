using FluentValidation;
using MF.OrderManagement.Domain.Entities.Payments;

namespace MF.OrderManagement.Domain.Validator.Payments;

public class PaymentConditionValidator : AbstractValidator<PaymentCondition>
{
    public PaymentConditionValidator()
    {
        RuleFor(pc => pc.Description)
            .NotEmpty()
            .WithMessage("Descrição da condição de pagamento é obrigatória")
            .MaximumLength(100)
            .WithMessage("Descrição não pode exceder 100 caracteres");

        RuleFor(pc => pc.NumberOfInstallments)
            .GreaterThan(0)
            .WithMessage("Quantidade de parcelas deve ser maior que zero");
            // .LessThanOrEqualTo(120)
            // .WithMessage("Quantidade de parcelas não pode ser maior que 120");

        // RuleFor(pc => pc.CreatedAt)
        //     .LessThanOrEqualTo(DateTime.UtcNow)
        //     .WithMessage("Data de criação não pode ser no futuro");
    }
}