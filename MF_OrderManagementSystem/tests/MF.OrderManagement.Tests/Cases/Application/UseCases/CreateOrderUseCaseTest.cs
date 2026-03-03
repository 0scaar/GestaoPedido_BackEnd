using FluentValidation;
using MF.OrderManagement.Application.Common.Abstractions;
using MF.OrderManagement.Application.Orders.DTOs;
using MF.OrderManagement.Application.Orders.Ports;
using MF.OrderManagement.Application.Orders.UseCases.CreateOrder;
using Moq;

namespace MF.OrderManagement.Tests.Cases.Application.UseCases;

public class CreateOrderUseCaseTest
{
    [Fact]
    public async Task DeveriaPublicarMensagem_QuandoCriarPedido()
    {
        var validator = new Mock<IValidator<CreateOrderRequest>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateOrderRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var orders = new Mock<IOrderRepository>();
        var customers = new Mock<ICustomerRepository>();
        var payments = new Mock<IPaymentConditionRepository>();
        var uow = new Mock<IUnitOfWork>();
        var bus = new Mock<IMessageBus>();
        var clock = new Mock<IDateTimeProvider>();
        clock.SetupGet(x => x.UtcNow).Returns(new DateTime(2026, 2, 25, 12, 0, 0, DateTimeKind.Utc));

        var useCase = new CreateOrderUseCase(
            validator.Object,
            orders.Object,
            customers.Object,
            payments.Object,
            uow.Object,
            bus.Object,
            clock.Object
        );

        var req = new CreateOrderRequest
        {
            Customer = new CreateCustomerRequest { Name = "Oscar", Email = "oscar@email.com" },
            PaymentCondition = new CreatePaymentConditionRequest { Description = "30", NumberOfInstallments = 1 },
            Items = new() { new CreateOrderItemRequest { ProductName = "A", Quantity = 1, UnitPrice = 10 } }
        };

        var result = await useCase.ExecuteAsync(req);

        bus.Verify(b => b.PublishAsync(
                It.Is<OrderCreatedMessage>(m => m.OrderId == result.OrderId),
                It.IsAny<CancellationToken>()),
            Times.Once);

        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}