using MF.OrderManagement.Domain.Entities.Customers;

namespace MF.OrderManagement.Application.Orders.Ports;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken ct = default);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default);
}