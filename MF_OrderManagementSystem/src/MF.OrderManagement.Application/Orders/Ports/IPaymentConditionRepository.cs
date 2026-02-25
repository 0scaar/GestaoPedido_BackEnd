using MF.OrderManagement.Domain.Entities.Payments;

namespace MF.OrderManagement.Application.Orders.Ports;

public interface IPaymentConditionRepository
{
    Task AddAsync(PaymentCondition paymentCondition, CancellationToken ct = default);
    Task<PaymentCondition?> GetByDescriptionAsync(string description, CancellationToken ct = default);
}