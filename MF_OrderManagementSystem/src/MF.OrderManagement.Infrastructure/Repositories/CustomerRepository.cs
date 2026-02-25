using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Customers;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Repositories;

public sealed class CustomerRepository(OrdersDbContext db) : ICustomerRepository
{
    public async Task AddAsync(Customer customer, CancellationToken ct = default)
        => await db.Customers.AddAsync(customer, ct);
    
    public Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default)
        => db.Customers.FirstOrDefaultAsync(x => x.Email == email, ct);
}