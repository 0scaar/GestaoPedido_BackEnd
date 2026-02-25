using MF.OrderManagement.Domain.Entities.Customers;

namespace MF.OrderManagement.Application.Orders.Ports;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid customerId, CancellationToken ct = default);
}