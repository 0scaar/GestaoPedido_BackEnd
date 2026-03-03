using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Queries;

public sealed class OrderQueries(OrdersDbContext db) : IOrderQueries
{
    public async Task<IReadOnlyList<OrderListItemDto>> ListAsync(CancellationToken ct = default)
    {
        return await db.Orders
            .AsNoTracking()
            .OrderBy(x => x.CreatedAt)
            .Join(
                db.Customers.AsNoTracking(),
                o => o.CustomerId,
                c => c.Id,
                (o, c) => new OrderListItemDto
                {
                    OrderId = o.Id,
                    CustomerName = c.Name,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    RequiresManualApproval = o.RequiresManualApproval
                }
            )
            .ToListAsync(ct);
    }

    public async Task<OrderListItemDto?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
    {
        return await db.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .Join(
                db.Customers.AsNoTracking(),
                o => o.CustomerId,
                c => c.Id,
                (o, c) => new OrderListItemDto
                {
                    OrderId = o.Id,
                    CustomerName = c.Name,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    RequiresManualApproval = o.RequiresManualApproval
                }
            )
            .FirstOrDefaultAsync(ct);
    }
}