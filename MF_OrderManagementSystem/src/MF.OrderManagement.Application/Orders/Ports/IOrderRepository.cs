using MF.OrderManagement.Domain.Entities.Orders;

namespace MF.OrderManagement.Application.Orders.Ports;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken ct = default);
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default);
    Task<IReadOnlyList<Order>> ListAsync(CancellationToken ct = default);
}