using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Domain.Entities.Payments;

public sealed class PaymentCondition : Entity
{
    public string Description { get; private set; }
    public int NumberOfInstallments { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public PaymentCondition(Guid id, string description, int numberOfInstallments) : base(id)
    {
        // if (string.IsNullOrWhiteSpace(description))
        //     throw new DomainException("Payment condition description is required.");
        //
        // if (numberOfInstallments <= 0)
        //     throw new DomainException("Number of installments must be greater than zero.");

        Description = description.Trim();
        NumberOfInstallments = numberOfInstallments;
        CreatedAt = DateTime.UtcNow;
    }
}