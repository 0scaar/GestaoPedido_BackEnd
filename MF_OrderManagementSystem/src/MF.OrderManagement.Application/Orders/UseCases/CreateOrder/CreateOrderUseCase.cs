using MF.OrderManagement.Application.Common.Abstractions;
using MF.OrderManagement.Application.Common.Exceptions;
using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Domain.Entities.Customers;
using MF.OrderManagement.Domain.Entities.Orders;
using MF.OrderManagement.Domain.Entities.Payments;

namespace MF.OrderManagement.Application.Orders.UseCases.CreateOrder;

public sealed class CreateOrderUseCase(
    IOrderRepository orders,
    ICustomerRepository customers,
    IPaymentConditionRepository paymentConditions,
    IUnitOfWork uow,
    IMessageBus bus,
    IDateTimeProvider clock)
{
    public async Task<CreateOrderResultDto> ExecuteAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        if (request.Customer is null) throw new BadRequest("Customer is required.");
        if (request.PaymentCondition is null) throw new BadRequest("PaymentCondition is required.");
        if (request.Items is null || request.Items.Count == 0) throw new BadRequest("Order must have at least one item.");
        
        if (string.IsNullOrWhiteSpace(request.Customer.Name)) throw new BadRequest("Customer.Name is required.");
        if (string.IsNullOrWhiteSpace(request.Customer.Email)) throw new BadRequest("Customer.Email is required.");
        if (string.IsNullOrWhiteSpace(request.PaymentCondition.Description)) throw new BadRequest("PaymentCondition.Description is required.");
        if (request.PaymentCondition.NumberOfInstallments <= 0) throw new BadRequest("PaymentCondition.NumberOfInstallments must be > 0.");
        
        var customer = await customers.GetByEmailAsync(request.Customer.Email, ct);
        if (customer is null)
        {
            customer = new Customer(
                id: Guid.NewGuid(),
                name: request.Customer.Name,
                email: request.Customer.Email
            );
            await customers.AddAsync(customer, ct);
        }
        
        var payment = await paymentConditions.GetByDescriptionAsync(request.PaymentCondition.Description, ct);
        if (payment is null)
        {
            payment = new PaymentCondition(
                id: Guid.NewGuid(),
                description: request.PaymentCondition.Description,
                numberOfInstallments: request.PaymentCondition.NumberOfInstallments
            );
            await paymentConditions.AddAsync(payment, ct);
        }

        // Cria aggregate no Domain (a regra dos 5000 fica lá)
        var order = Order.Create(
            id: Guid.NewGuid(),
            customerId: customer.Id,
            paymentConditionId: payment.Id,
            orderDate: clock.UtcNow,
            items: request.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice))
        );

        await orders.AddAsync(order, ct);
        await uow.SaveChangesAsync(ct);

        // Publica mensagem para o Worker
        await bus.PublishAsync(new OrderCreatedMessage
        {
            OrderId = order.Id,
            OccurredAtUtc = clock.UtcNow
        }, ct);

        return new CreateOrderResultDto
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),
            RequiresManualApproval = order.RequiresManualApproval
        };
    }
    
    private sealed class BadRequest : ApplicationExceptionBase
    {
        public BadRequest(string message) : base(message) { }
    }
}

