namespace MF.OrderManagement.Application.Orders.DTOs;

public sealed class CreateOrderResultDto
{
    public Guid OrderId { get; init; }
    public decimal TotalAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool RequiresManualApproval { get; init; }
}