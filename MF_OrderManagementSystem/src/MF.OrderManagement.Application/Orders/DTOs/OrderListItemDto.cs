namespace MF.OrderManagement.Application.Orders.DTOs;

public sealed class OrderListItemDto
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool RequiresManualApproval { get; init; }
}