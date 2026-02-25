using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Payments;
using MF.OrderManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Repositories;

public sealed class PaymentConditionRepository(OrdersDbContext db) : IPaymentConditionRepository
{
    public async Task AddAsync(PaymentCondition paymentCondition, CancellationToken ct = default)
        => await db.PaymentConditions.AddAsync(paymentCondition, ct);

    public Task<PaymentCondition?> GetByDescriptionAsync(string description, CancellationToken ct = default)
        => db.PaymentConditions.FirstOrDefaultAsync(x => x.Description == description, ct);
}