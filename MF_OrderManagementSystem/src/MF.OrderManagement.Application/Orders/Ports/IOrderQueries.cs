using MF.OrderManagement.Application.Orders.DTOs;

namespace MF.OrderManagement.Application.Orders.Ports;

public interface IOrderQueries
{
    Task<IReadOnlyList<OrderListItemDto>> ListAsync(CancellationToken ct = default);
    Task<OrderListItemDto?> GetByIdAsync(Guid orderId, CancellationToken ct = default);
}