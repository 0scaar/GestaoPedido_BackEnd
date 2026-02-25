using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Orders;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Repositories;

public sealed class OrderRepository(OrdersDbContext db) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken ct = default)
        => await db.Orders.AddAsync(order, ct);

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
        => db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

    public async Task<IReadOnlyList<Order>> ListAsync(CancellationToken ct = default)
        => await db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(ct);
}