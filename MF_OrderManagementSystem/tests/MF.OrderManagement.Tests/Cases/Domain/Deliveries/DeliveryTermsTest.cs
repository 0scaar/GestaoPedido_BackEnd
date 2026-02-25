using FluentAssertions;
using MF.OrderManagement.Domain.Validator.Deliveries;
using MF.OrderManagement.Tests.Builders.Deliveries;

namespace MF.OrderManagement.Tests.Cases.Domain.Deliveries;

public class DeliveryTermsTest
{
    private readonly DeliveryTermsValidator _validator = new DeliveryTermsValidator();

    [Fact]
    public void Should_Create_Valid_DeliveryTerms()
    {
        var deliveryTerms = DeliveryTermsBuilder.New().Build();
        var result = _validator.Validate(deliveryTerms);
        
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void OrderId_Is_Required()
    {
        var deliveryTerms = DeliveryTermsBuilder.New()
            .WithOrderId(Guid.Empty)
            .Build();
        var result = _validator.Validate(deliveryTerms);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain("Id do pedido é obrigatório");
    }

    [Fact]
    public void EstimatedDeliveryDate_Is_Required()
    {
        var deliveryTerms = DeliveryTermsBuilder.New()
            .WithEstimatedDeliveryDate(DateTime.MinValue)
            .Build();
        var result = _validator.Validate(deliveryTerms);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain("Data estimada de entrega é obrigatória");
    }

    [Fact]
    public void EstimatedDeliveryDate_Must_Be_In_Future()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var deliveryTerms = DeliveryTermsBuilder.New()
            .WithEstimatedDeliveryDate(pastDate)
            .Build();
        var result = _validator.Validate(deliveryTerms);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain("Data estimada de entrega deve ser no futuro");
    }

    [Fact]
    public void DeliveryDays_Must_Be_Greater_Than_Zero()
    {
        var deliveryTerms = DeliveryTermsBuilder.New()
            .WithDeliveryDays(0)
            .Build();
        var result = _validator.Validate(deliveryTerms);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain("Prazo em dias deve ser maior que zero");
    }

    [Fact]
    public void DeliveryDays_Negative_Value_Should_Fail()
    {
        var deliveryTerms = DeliveryTermsBuilder.New()
            .WithDeliveryDays(-5)
            .Build();
        var result = _validator.Validate(deliveryTerms);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage)
            .Should().Contain("Prazo em dias deve ser maior que zero");
    }

    [Fact]
    public void Should_Create_DeliveryTerms_With_Custom_Values()
    {
        var customId = Guid.NewGuid();
        var customOrderId = Guid.NewGuid();
        var customDate = DateTime.UtcNow.AddDays(15);
        var customDays = 15;

        var deliveryTerms = DeliveryTermsBuilder.New()
            .WithId(customId)
            .WithOrderId(customOrderId)
            .WithEstimatedDeliveryDate(customDate)
            .WithDeliveryDays(customDays)
            .Build();

        deliveryTerms.Id.Should().Be(customId);
        deliveryTerms.OrderId.Should().Be(customOrderId);
        deliveryTerms.EstimatedDeliveryDate.Should().Be(customDate);
        deliveryTerms.DeliveryDays.Should().Be(customDays);
    }
}