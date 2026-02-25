using MF.OrderManagement.Domain.Entities.Payments;

namespace MF.OrderManagement.Application.Orders.Ports;

public interface IPaymentConditionRepository
{
    Task<PaymentCondition?> GetByIdAsync(Guid paymentConditionId, CancellationToken ct = default);
}