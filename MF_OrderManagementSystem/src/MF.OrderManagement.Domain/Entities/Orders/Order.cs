using MF.OrderManagement.Domain.Common;
using MF.OrderManagement.Domain.Entities.Orders.Events;

namespace MF.OrderManagement.Domain.Entities.Orders;

public sealed class Order : AggregateRoot
{
    public Guid CustomerId { get; private set; }
    public Guid PaymentConditionId { get; private set; }
    public DateTime OrderDate { get; private set; }

    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public bool RequiresManualApproval { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private const decimal ManualApprovalThreshold = 5000m;

    public Order(Guid id, 
        Guid customerId, 
        Guid paymentConditionId,
        DateTime orderDate) : base(id)
    {
        CustomerId = customerId;
        PaymentConditionId = paymentConditionId;
        OrderDate = orderDate;
        CreatedAt = DateTime.UtcNow;
        
        TotalAmount = 0m;
        Status = OrderStatus.Created;
        RequiresManualApproval = false;
    }
    
    public static Order Create(
        Guid id,
        Guid customerId,
        Guid paymentConditionId,
        DateTime orderDate,
        IEnumerable<(string ProductName, int Quantity, decimal UnitPrice)> items
    )
    {
        var order = new Order(
            id: id == Guid.Empty ? Guid.NewGuid() : id,
            customerId: customerId,
            paymentConditionId: paymentConditionId,
            orderDate: orderDate
        );

        foreach (var item in items ?? Enumerable.Empty<(string, int, decimal)>())
            order.AddItem(item.ProductName, item.Quantity, item.UnitPrice);

        order.RecalculateTotalsAndApplyBusinessRules();

        // Evento de domínio: pedido criado
        order.AddDomainEvent(DomainEvent.Create((eventId, occurredAt) =>
            new OrderCreatedDomainEvent(eventId, occurredAt, order.Id)));

        return order;
    }
    
    public void Approve()
    {
        if (!RequiresManualApproval)
            throw new DomainException("Order does not require manual approval.");

        if (Status != OrderStatus.Created)
            throw new DomainException("Only orders in 'Created' status can be approved.");

        Status = OrderStatus.Paid;
        RequiresManualApproval = false;
    }
    
    private void AddItem(string productName, int quantity, decimal unitPrice)
    {
        var orderItem = new OrderItem(
            id: Guid.NewGuid(),
            orderId: Id,
            productName: productName,
            quantity: quantity,
            unitPrice: unitPrice
        );

        _items.Add(orderItem);
    }

    private void RecalculateTotalsAndApplyBusinessRules()
    {
        if (_items.Count == 0)
            throw new DomainException("Order must have at least one item.");

        TotalAmount = _items.Sum(i => i.TotalPrice);

        if (TotalAmount > ManualApprovalThreshold)
        {
            Status = OrderStatus.Created;
            RequiresManualApproval = true;
        }
        else
        {
            Status = OrderStatus.Paid;
            RequiresManualApproval = false;
        }
    }
}