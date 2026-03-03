using MF.OrderManagement.Application.Common.Exceptions;
using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;

namespace MF.OrderManagement.Application.Orders.UseCases.GetOrders;

public class GetOrderByIdUseCase(IOrderQueries queries)
{
    public async Task<OrderListItemDto> ExecuteAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await queries.GetByIdAsync(orderId, ct);
        if (order is null)
            throw new NotFoundException($"Order '{orderId}' not found.");

        return order;
    }
}