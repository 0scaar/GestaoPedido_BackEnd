using MF.OrderManagement.Domain.Entities.Customers;
using MF.OrderManagement.Domain.Entities.Deliveries;
using MF.OrderManagement.Domain.Entities.Orders;
using MF.OrderManagement.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;

namespace MF.OrderManagement.Infrastructure.Persistence;

public sealed class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<PaymentCondition> PaymentConditions => Set<PaymentCondition>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<DeliveryTerms> DeliveryTerms => Set<DeliveryTerms>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
    }
}