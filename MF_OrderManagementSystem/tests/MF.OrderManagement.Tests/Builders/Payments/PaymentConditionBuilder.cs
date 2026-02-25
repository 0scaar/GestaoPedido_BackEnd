using MF.OrderManagement.Domain.Entities.Payments;

namespace MF.OrderManagement.Tests.Builders.Payments;

public class PaymentConditionBuilder
{
    private Guid _id = Guid.NewGuid();
    private string _description = "Net 30";
    private int _numberOfInstallments = 1;

    public static PaymentConditionBuilder New() => new PaymentConditionBuilder();

    public PaymentConditionBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PaymentConditionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public PaymentConditionBuilder WithNumberOfInstallments(int numberOfInstallments)
    {
        _numberOfInstallments = numberOfInstallments;
        return this;
    }

    public PaymentCondition Build()
    {
        var paymentCondition = new PaymentCondition(_id, _description, _numberOfInstallments);
        return paymentCondition;
    }
}