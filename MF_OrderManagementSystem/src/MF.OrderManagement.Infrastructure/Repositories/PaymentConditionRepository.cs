using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Payments;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Repositories;

public sealed class PaymentConditionRepository(OrdersDbContext db) : IPaymentConditionRepository
{
    public Task<PaymentCondition?> GetByIdAsync(Guid paymentConditionId, CancellationToken ct = default)
        => db.PaymentConditions.FirstOrDefaultAsync(x => x.Id == paymentConditionId, ct);
}