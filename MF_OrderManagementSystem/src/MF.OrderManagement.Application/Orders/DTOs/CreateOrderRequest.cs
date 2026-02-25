namespace MF.OrderManagement.Application.Orders.DTOs;

public sealed class CreateOrderRequest
{
    public Guid CustomerId { get; init; }
    public Guid PaymentConditionId { get; init; }
    public List<CreateOrderItemRequest> Items { get; init; } = new();
}

public sealed class CreateOrderItemRequest
{
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}