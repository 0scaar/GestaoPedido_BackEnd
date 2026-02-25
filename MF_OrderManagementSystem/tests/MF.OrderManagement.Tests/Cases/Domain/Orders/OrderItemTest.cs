using FluentAssertions;
using MF.OrderManagement.Domain.Validator.Orders;
using MF.OrderManagement.Tests.Builders.Orders;

namespace MF.OrderManagement.Tests.Cases.Domain.Orders;

public class OrderItemTest
{
    private readonly OrderItemValidator _validator = new OrderItemValidator();

    [Fact]
    public void Should_Create_Valid_OrderItem()
    {
        var item = OrderItemBuilder.New().Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void OrderId_Is_Required()
    {
        var item = OrderItemBuilder.New().WithOrderId(Guid.Empty).Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Id do pedido é obrigatório");
    }

    [Fact]
    public void ProductName_Is_Required()
    {
        var item = OrderItemBuilder.New().WithProductName(string.Empty).Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Nome do produto é obrigatório");
    }

    [Fact]
    public void ProductName_MaxLength_Exceeded()
    {
        var longName = new string('A', 151);
        var item = OrderItemBuilder.New().WithProductName(longName).Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Nome do produto não pode exceder 150 caracteres");
    }

    [Fact]
    public void Quantity_Must_Be_Greater_Than_Zero()
    {
        var item = OrderItemBuilder.New().WithQuantity(0).Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Quantidade deve ser maior que zero");
    }

    [Fact]
    public void UnitPrice_Must_Be_Greater_Than_Zero()
    {
        var item = OrderItemBuilder.New().WithUnitPrice(0m).Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Preço unitário deve ser maior que zero");
    }

    [Fact]
    public void TotalPrice_Must_Be_Greater_Than_Zero()
    {
        var item = OrderItemBuilder.New().WithQuantity(0).WithUnitPrice(0m).Build();
        var result = _validator.Validate(item);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Preço total deve ser maior que zero");
    }

    [Fact]
    public void Should_Create_OrderItem_With_Custom_Values_And_TotalPrice_Calculated()
    {
        var id = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var qty = 5;
        var unit = 12.5m;

        var item = OrderItemBuilder.New()
            .WithId(id)
            .WithOrderId(orderId)
            .WithQuantity(qty)
            .WithUnitPrice(unit)
            .WithProductName("Custom Product")
            .Build();

        item.Id.Should().Be(id);
        item.OrderId.Should().Be(orderId);
        item.ProductName.Should().Be("Custom Product");
        item.Quantity.Should().Be(qty);
        item.UnitPrice.Should().Be(unit);
        item.TotalPrice.Should().Be(qty * unit);

        var result = _validator.Validate(item);
        result.IsValid.Should().BeTrue();
    }
}