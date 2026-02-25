using MF.OrderManagement.Domain.Entities.Orders;

namespace MF.OrderManagement.Tests.Builders.Orders;

public class OrderBuilder
{
    private Guid _id = Guid.NewGuid();
    private Guid _customerId = Guid.NewGuid();
    private Guid _paymentConditionId = Guid.NewGuid();
    private DateTime _orderDate = DateTime.UtcNow.AddHours(-4);
    private decimal _totalAmount = 0m;
    private OrderStatus _status = OrderStatus.Created;
    private bool _requiresManualApproval =  false;

    public static OrderBuilder New() => new OrderBuilder();

    public OrderBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public OrderBuilder WithCustomerId(Guid customerId)
    {
        _customerId = customerId;
        return this;
    }

    public OrderBuilder WithPaymentConditionId(Guid paymentConditionId)
    {
        _paymentConditionId = paymentConditionId;
        return this;
    }

    public OrderBuilder WithOrderDate(DateTime orderDate)
    {
        _orderDate = orderDate;
        return this;
    }

    public OrderBuilder WithTotalAmount(decimal totalAmount)
    {
        _totalAmount = totalAmount;
        return this;
    }

    public OrderBuilder WithStatus(OrderStatus status)
    {
        _status = status;
        return this;
    }

    public OrderBuilder WithRequiresManualApproval(bool requiresManualApproval)
    {
        _requiresManualApproval = requiresManualApproval;
        return this;
    }

    public Order Build()
    {
        var order = new Order(_id, _customerId, _paymentConditionId, _orderDate);
        return order;
    }
}