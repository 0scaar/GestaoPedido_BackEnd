using MF.OrderManagement.Domain.Common;

namespace MF.OrderManagement.Domain.Entities.Customers;

public sealed class Customer : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Customer(Guid id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }
}