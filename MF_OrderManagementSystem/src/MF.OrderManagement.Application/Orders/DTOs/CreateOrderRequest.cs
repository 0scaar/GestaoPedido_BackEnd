namespace MF.OrderManagement.Application.Orders.DTOs;

public sealed class CreateOrderRequest
{
    public CreateCustomerRequest Customer { get; init; } = new();
    public CreatePaymentConditionRequest PaymentCondition { get; init; } = new();
    public List<CreateOrderItemRequest> Items { get; init; } = new();
}

public sealed class CreateCustomerRequest
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}

public sealed class CreatePaymentConditionRequest
{
    public string Description { get; init; } = string.Empty;
    public int NumberOfInstallments { get; init; }
}

public sealed class CreateOrderItemRequest
{
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}