using MF.OrderManagement.Domain.Entities.Orders;

namespace MF.OrderManagement.Tests.Builders.Orders;

public class OrderItemBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _orderId = Guid.NewGuid();
    private string _productName = "Sample Product";
    private int _quantity = 1;
    private decimal _unitPrice = 10m;
    public static OrderItemBuilder New() => new OrderItemBuilder();

    public OrderItemBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public OrderItemBuilder WithOrderId(Guid orderId)
    {
        _orderId = orderId;
        return this;
    }

    public OrderItemBuilder WithProductName(string productName)
    {
        _productName = productName;
        return this;
    }

    public OrderItemBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderItemBuilder WithUnitPrice(decimal unitPrice)
    {
        _unitPrice = unitPrice;
        return this;
    }

    public OrderItem Build()
    {
        var orderItem = new OrderItem(_id, _orderId, _productName, _quantity, _unitPrice);
        return orderItem;
    }
}