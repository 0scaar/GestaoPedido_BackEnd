using System.Reflection;
using MF.OrderManagement.Domain.Entities.Customers;

namespace MF.OrderManagement.Tests.Builders.Customers;

public class CustomerBuilder
{
	private Guid _id = Guid.NewGuid();
	private string _name = "John Doe";
	private string _email = "john.doe@example.com";

	public static CustomerBuilder New() => new CustomerBuilder();

	public CustomerBuilder WithId(Guid id)
	{
		_id = id;
		return this;
	}

	public CustomerBuilder WithName(string name)
	{
		_name = name;
		return this;
	}

	public CustomerBuilder WithEmail(string email)
	{
		_email = email;
		return this;
	}

	public Customer Build()
	{
		var customer = new Customer(_id, _name, _email);

		return customer;
	}
}
