using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Domain.Entities.Orders;

public sealed class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }

    public OrderItem(Guid id, 
        Guid orderId, 
        string productName, 
        int quantity, 
        decimal unitPrice) : base(id)
    {
        OrderId = orderId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        
        TotalPrice = checked(quantity * unitPrice);
    }
}