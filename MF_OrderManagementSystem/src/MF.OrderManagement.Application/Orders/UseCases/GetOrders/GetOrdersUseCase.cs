using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;

namespace MF.OrderManagement.Application.Orders.UseCases.GetOrders;

public class GetOrdersUseCase
{
    private readonly IOrderRepository _orders;

    public GetOrdersUseCase(IOrderRepository orders)
    {
        _orders = orders;
    }
    
    public async Task<IReadOnlyList<OrderListItemDto>> ExecuteAsync(CancellationToken ct = default)
    {
        var orders = await _orders.ListAsync(ct);

        return orders.Select(o => new OrderListItemDto
        {
            OrderId = o.Id,
            CustomerId = o.CustomerId,
            TotalAmount = o.TotalAmount,
            Status = o.Status.ToString(),
            RequiresManualApproval = o.RequiresManualApproval
        }).ToList();
    }
}