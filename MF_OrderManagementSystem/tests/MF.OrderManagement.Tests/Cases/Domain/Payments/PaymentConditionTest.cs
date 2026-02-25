using FluentAssertions;
using MF.OrderManagement.Domain.Validator.Payments;
using MF.OrderManagement.Tests.Builders.Payments;

namespace MF.OrderManagement.Tests.Cases.Domain.Payments;

public class PaymentConditionTest
{
    private readonly PaymentConditionValidator _validator = new PaymentConditionValidator();

    [Fact]
    public void Should_Create_Valid_PaymentCondition()
    {
        var pc = PaymentConditionBuilder.New().Build();
        var result = _validator.Validate(pc);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Description_Is_Required()
    {
        var pc = PaymentConditionBuilder.New().WithDescription(string.Empty).Build();
        var result = _validator.Validate(pc);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Descrição da condição de pagamento é obrigatória");
    }

    [Fact]
    public void Description_MaxLength_Exceeded()
    {
        var longDesc = new string('A', 101);
        var pc = PaymentConditionBuilder.New().WithDescription(longDesc).Build();
        var result = _validator.Validate(pc);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Descrição não pode exceder 100 caracteres");
    }

    [Fact]
    public void NumberOfInstallments_Must_Be_Greater_Than_Zero()
    {
        var pc = PaymentConditionBuilder.New().WithNumberOfInstallments(0).Build();
        var result = _validator.Validate(pc);
        result.IsValid.Should().BeFalse();
        result.Errors.Select(e => e.ErrorMessage).Should().Contain("Quantidade de parcelas deve ser maior que zero");
    }

    [Fact]
    public void Description_Should_Be_Trimmed()
    {
        var pc = PaymentConditionBuilder.New().WithDescription("  Net 30  ").Build();
        pc.Description.Should().Be("Net 30");
    }

    [Fact]
    public void Should_Create_With_Custom_Values()
    {
        var id = Guid.NewGuid();
        var desc = "Custom";
        var installments = 12;

        var pc = PaymentConditionBuilder.New()
            .WithId(id)
            .WithDescription(desc)
            .WithNumberOfInstallments(installments)
            .Build();

        pc.Id.Should().Be(id);
        pc.Description.Should().Be(desc);
        pc.NumberOfInstallments.Should().Be(installments);
    }
}
