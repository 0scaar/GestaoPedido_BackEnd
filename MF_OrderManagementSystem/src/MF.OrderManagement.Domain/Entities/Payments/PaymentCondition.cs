using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Domain.Entities.Payments;

public sealed class PaymentCondition : Entity
{
    public string Description { get; private set; }
    public int NumberOfInstallments { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public PaymentCondition(Guid id, string description, int numberOfInstallments) : base(id)
    {
        Description = description.Trim();
        NumberOfInstallments = numberOfInstallments;
        CreatedAt = DateTime.UtcNow;
    }
}