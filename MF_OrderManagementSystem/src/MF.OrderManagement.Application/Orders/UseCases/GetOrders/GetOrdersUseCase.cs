using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;

namespace MF.OrderManagement.Application.Orders.UseCases.GetOrders;

public class GetOrdersUseCase(IOrderQueries queries)
{
    public Task<IReadOnlyList<OrderListItemDto>> ExecuteAsync(CancellationToken ct = default)
        => queries.ListAsync(ct);
}