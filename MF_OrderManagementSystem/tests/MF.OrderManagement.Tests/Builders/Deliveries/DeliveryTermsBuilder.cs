using System.Reflection;
using MF.OrderManagement.Domain.Entities.Deliveries;

namespace MF.OrderManagement.Tests.Builders.Deliveries;

public class DeliveryTermsBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _orderId = Guid.NewGuid();
    private DateTime _estimatedDeliveryDate = DateTime.UtcNow.AddDays(7);
    private int _deliveryDays = 7;
    
    public static DeliveryTermsBuilder New() => new DeliveryTermsBuilder();

    public DeliveryTermsBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public DeliveryTermsBuilder WithOrderId(Guid orderId)
    {
        _orderId = orderId;
        return this;
    }

    public DeliveryTermsBuilder WithEstimatedDeliveryDate(DateTime estimatedDeliveryDate)
    {
        _estimatedDeliveryDate = estimatedDeliveryDate;
        return this;
    }

    public DeliveryTermsBuilder WithDeliveryDays(int deliveryDays)
    {
        _deliveryDays = deliveryDays;
        return this;
    }

    public DeliveryTerms Build()
    {
        var deliveryTerms = new DeliveryTerms(_id, _orderId, _estimatedDeliveryDate, _deliveryDays);

        return deliveryTerms;
    }
}
