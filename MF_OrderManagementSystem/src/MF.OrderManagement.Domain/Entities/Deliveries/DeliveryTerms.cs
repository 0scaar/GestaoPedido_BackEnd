using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Domain.Entities.Deliveries;

public sealed class DeliveryTerms : Entity
{
    public Guid OrderId { get; private set; }
    public DateTime EstimatedDeliveryDate  { get; private set; }
    public int DeliveryDays  { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public DeliveryTerms(Guid id, Guid orderId, DateTime estimatedDeliveryDate, int deliveryDays) : base(id)
    {
        OrderId = orderId;
        EstimatedDeliveryDate = estimatedDeliveryDate;
        DeliveryDays = deliveryDays;
        CreatedAt = DateTime.UtcNow;
    }
}