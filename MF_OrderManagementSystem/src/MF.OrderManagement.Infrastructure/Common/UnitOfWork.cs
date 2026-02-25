using MF.OrderManagement.Application.Common.Abstractions;
using MF.OrderManagement.Infrastructure.Persistence;

namespace MF.OrderManagement.Infrastructure.Common;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _db;

    public UnitOfWork(OrdersDbContext db) => _db = db;
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}