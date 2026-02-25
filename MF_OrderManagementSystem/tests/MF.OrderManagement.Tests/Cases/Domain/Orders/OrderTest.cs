using FluentAssertions;
using MF.OrderManagement.Domain.Common;
using MF.OrderManagement.Domain.Entities.Orders;
using MF.OrderManagement.Domain.Validator.Orders;
using MF.OrderManagement.Tests.Builders.Orders;

namespace MF.OrderManagement.Tests.Cases.Domain.Orders;

public class OrderTest
{
    private readonly OrderValidator _validator = new OrderValidator();

    // [Fact]
    // public void Should_Create_Valid_Order()
    // {
    //     var order = OrderBuilder.New().Build();
    //     var result = _validator.Validate(order);
    //     result.IsValid.Should().BeTrue();
    // }

    [Fact]
    public void CustomerId_Is_Required()
    {
        var order = OrderBuilder.New().WithCustomerId(Guid.Empty).Build();
        var result = _validator.Validate(order);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Id do cliente é obrigatório");
    }

    [Fact]
    public void PaymentConditionId_Is_Required()
    {
        var order = OrderBuilder.New().WithPaymentConditionId(Guid.Empty).Build();
        var result = _validator.Validate(order);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Id da condição de pagamento é obrigatório");
    }

    [Fact]
    public void OrderDate_Cannot_Be_In_Future()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);
        var order = OrderBuilder.New().WithOrderDate(futureDate).Build();
        var result = _validator.Validate(order);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Data do pedido não pode ser no futuro");
    }

    [Fact]
    public void TotalAmount_Cannot_Be_Negative()
    {
        var items = new[]
        {
            ("Product A", 2, -50m),
            ("Product B", 3, 30m)
        };

        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, items);
        var result = _validator.Validate(order);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Valor total não pode ser negativo");
    }

    [Fact]
    public void Create_With_Items_Should_Calculate_TotalAmount()
    {
        var items = new[]
        {
            ("Product A", 2, 50m),
            ("Product B", 3, 30m)
        };

        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, items);

        order.TotalAmount.Should().Be(190m); // (2*50) + (3*30) = 100 + 90 = 190
        order.Items.Count.Should().Be(2);
        order.Status.Should().Be(OrderStatus.Paid);
        order.RequiresManualApproval.Should().BeFalse();
    }

    [Fact]
    public void Create_With_Items_Exceeding_Threshold_Should_Require_Approval()
    {
        var items = new[]
        {
            ("Product A", 100, 60m)
        };

        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, items);

        order.TotalAmount.Should().Be(6000m); // 100*60 = 6000 (exceeds 5000 threshold)
        order.Status.Should().Be(OrderStatus.Created);
        order.RequiresManualApproval.Should().BeTrue();
    }

    [Fact]
    public void Approve_Should_Change_Status_To_Paid()
    {
        var items = new[]
        {
            ("Product A", 100, 60m)
        };

        var order = Order.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, items);
        order.RequiresManualApproval.Should().BeTrue();

        order.Approve();

        order.Status.Should().Be(OrderStatus.Paid);
        order.RequiresManualApproval.Should().BeFalse();
    }

    // [Fact]
    // public void Approve_Should_Fail_When_Not_Requires_Manual_Approval()
    // {
    //     var order = OrderBuilder.New().Build();

    //     var action = () => order.Approve();

    //     action.Should().Throw<DomainException>()
    //         .WithMessage("Order does not require manual approval.");
    // }

    // [Fact]
    // public void Create_Without_Items_Should_Throw_Exception()
    // {
    //     var action = () => Order.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Enumerable.Empty<(string, int, decimal)>());

    //     action.Should().Throw<DomainException>()
    //         .WithMessage("Order must have at least one item.");
    // }

    [Fact]
    public void Items_Should_Be_ReadOnly()
    {
        var order = OrderBuilder.New().Build();
        var items = order.Items;

        items.Should().NotBeNull();
        items.Should().BeEmpty();
    }

    // [Fact]
    // public void OrderDate_Should_Be_Set_To_UtcNow()
    // {
    //     var before = DateTime.UtcNow;
    //     var order = OrderBuilder.New().Build();
    //     var after = DateTime.UtcNow;

    //     order.OrderDate.Should().BeGreaterThanOrEqualTo(before);
    //     order.OrderDate.Should().BeLessThanOrEqualTo(after);
    // }

    // [Fact]
    // public void CreatedAt_Should_Be_Set_Automatically()
    // {
    //     var before = DateTime.UtcNow;
    //     var order = OrderBuilder.New().Build();
    //     var after = DateTime.UtcNow;

    //     order.CreatedAt.Should().BeGreaterThanOrEqualTo(before);
    //     order.CreatedAt.Should().BeLessThanOrEqualTo(after);
    // }
}