using MF.OrderManagement.Application.Common.Abstractions;
using MF.OrderManagement.Application.Common.Exceptions;
using MF.OrderManagement.Application.Orders.Ports;

namespace MF.OrderManagement.Application.Orders.UseCases.ApproveOrder;

public sealed class ApproveOrderUseCase
{
    private readonly IOrderRepository _orders;
    private readonly IUnitOfWork _uow;

    public ApproveOrderUseCase(IOrderRepository orders, IUnitOfWork uow)
    {
        _orders = orders;
        _uow = uow;
    }
    
    public async Task ExecuteAsync(Guid orderId, CancellationToken ct = default)
    {
        if (orderId == Guid.Empty)
            throw new ApplicationExceptionBaseImpl("OrderId is required.");

        var order = await _orders.GetByIdAsync(orderId, ct);
        if (order is null)
            throw new NotFoundException($"Order '{orderId}' not found.");

        order.Approve(); // regra no Domain

        await _uow.SaveChangesAsync(ct);
    }

    private sealed class ApplicationExceptionBaseImpl : ApplicationExceptionBase
    {
        public ApplicationExceptionBaseImpl(string message) : base(message) { }
    }
}