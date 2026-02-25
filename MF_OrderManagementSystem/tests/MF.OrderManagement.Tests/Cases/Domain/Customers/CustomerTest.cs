using FluentAssertions;
using MF.OrderManagement.Domain.Validator.Customers;
using MF.OrderManagement.Tests.Builders.Customers;

namespace MF.OrderManagement.Tests.Cases.Domain.Customers;

public class CustomerTest
{
	private readonly CustomerValidator _validator = new CustomerValidator();

	[Fact]
	public void Should_Create_Valid_Customer()
	{
		var customer = CustomerBuilder.New().Build();
		var result = _validator.Validate(customer);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Name_Is_Required()
	{
		var customer = CustomerBuilder.New().WithName(string.Empty).Build();
		var result = _validator.Validate(customer);
		result.IsValid.Should().BeFalse();
		result.Errors.Select(e => e.ErrorMessage).Should().Contain("Nome do cliente é obrigatório");
	}

	[Fact]
	public void Name_MaxLength_Exceeded()
	{
		var longName = new string('A', 151);
		var customer = CustomerBuilder.New().WithName(longName).Build();
		var result = _validator.Validate(customer);
		result.IsValid.Should().BeFalse();
		result.Errors.Select(e => e.ErrorMessage).Should().Contain("Nome do cliente não pode exceder 150 caracteres");
	}

	[Fact]
	public void Email_Is_Required()
	{
		var customer = CustomerBuilder.New().WithEmail(string.Empty).Build();
		var result = _validator.Validate(customer);
		result.IsValid.Should().BeFalse();
		result.Errors.Select(e => e.ErrorMessage).Should().Contain("Email é obrigatório");
	}

	[Fact]
	public void Email_Invalid_Format()
	{
		var customer = CustomerBuilder.New().WithEmail("notanemail").Build();
		var result = _validator.Validate(customer);
		result.IsValid.Should().BeFalse();
		result.Errors.Select(e => e.ErrorMessage).Should().Contain("Email deve ser um endereço válido");
	}

	[Fact]
	public void Email_MaxLength_Exceeded()
	{
		var longLocal = new string('a', 140);
		var longEmail = longLocal + "@example.com"; 
		var customer = CustomerBuilder.New().WithEmail(longEmail).Build();
		var result = _validator.Validate(customer);
		result.IsValid.Should().BeFalse();
		result.Errors.Select(e => e.ErrorMessage).Should().Contain("Email não pode exceder 150 caracteres");
	}
}
