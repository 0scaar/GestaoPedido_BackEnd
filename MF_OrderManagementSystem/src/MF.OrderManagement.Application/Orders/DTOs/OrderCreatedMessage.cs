namespace MF.OrderManagement.Application.Orders.DTOs;

public sealed class OrderCreatedMessage
{
    public Guid OrderId { get; init; }
    public DateTime OccurredAtUtc { get; init; }
}