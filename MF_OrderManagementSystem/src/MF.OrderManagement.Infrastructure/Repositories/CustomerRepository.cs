using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Customers;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Repositories;

public sealed class CustomerRepository(OrdersDbContext db) : ICustomerRepository
{
    public Task<Customer?> GetByIdAsync(Guid customerId, CancellationToken ct = default)
        => db.Customers.FirstOrDefaultAsync(x => x.Id == customerId, ct);
}