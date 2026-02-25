using MF.OrderManagement.Application.Common.Abstractions;
using MF.OrderManagement.Application.Common.Exceptions;
using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Orders;

namespace MF.OrderManagement.Application.Orders.UseCases.CreateOrder;

public sealed class CreateOrderUseCase
{
    private readonly IOrderRepository _orders;
    private readonly ICustomerRepository _customers;
    private readonly IPaymentConditionRepository _paymentConditions;
    private readonly IUnitOfWork _uow;
    private readonly IMessageBus _bus;
    private readonly IDateTimeProvider _clock;

    public CreateOrderUseCase(
        IOrderRepository orders,
        ICustomerRepository customers,
        IPaymentConditionRepository paymentConditions,
        IUnitOfWork uow,
        IMessageBus bus,
        IDateTimeProvider clock)
    {
        _orders = orders;
        _customers = customers;
        _paymentConditions = paymentConditions;
        _uow = uow;
        _bus = bus;
        _clock = clock;
    }
    
    public async Task<CreateOrderResultDto> ExecuteAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        // Validações básicas na Application (rápidas)
        if (request.CustomerId == Guid.Empty)
            throw new ApplicationExceptionBaseImpl("CustomerId is required.");

        if (request.PaymentConditionId == Guid.Empty)
            throw new ApplicationExceptionBaseImpl("PaymentConditionId is required.");

        if (request.Items is null || request.Items.Count == 0)
            throw new ApplicationExceptionBaseImpl("Order must have at least one item.");

        // Valida se referencias existem (opcional, mas bom)
        var customer = await _customers.GetByIdAsync(request.CustomerId, ct);
        if (customer is null)
            throw new NotFoundException($"Customer '{request.CustomerId}' not found.");

        var payment = await _paymentConditions.GetByIdAsync(request.PaymentConditionId, ct);
        if (payment is null)
            throw new NotFoundException($"PaymentCondition '{request.PaymentConditionId}' not found.");

        // Cria aggregate no Domain (a regra dos 5000 fica lá)
        var order = Order.Create(
            id: Guid.NewGuid(),
            customerId: request.CustomerId,
            paymentConditionId: request.PaymentConditionId,
            orderDate: _clock.UtcNow,
            items: request.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice))
        );

        await _orders.AddAsync(order, ct);
        await _uow.SaveChangesAsync(ct);

        // Publica mensagem para o Worker
        await _bus.PublishAsync(new OrderCreatedMessage
        {
            OrderId = order.Id,
            OccurredAtUtc = _clock.UtcNow
        }, ct);

        return new CreateOrderResultDto
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),
            RequiresManualApproval = order.RequiresManualApproval
        };
    }
    
    // Exceção simples só pra não criar mil classes agora
    private sealed class ApplicationExceptionBaseImpl : ApplicationExceptionBase
    {
        public ApplicationExceptionBaseImpl(string message) : base(message) { }
    }
}

