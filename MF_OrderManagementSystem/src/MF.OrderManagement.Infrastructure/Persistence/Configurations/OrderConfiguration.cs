using MF.OrderManagement.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MF.OrderManagement.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("OrderId");

        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.PaymentConditionId).IsRequired();

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>() // grava "Created"/"Paid"
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.RequiresManualApproval)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Importante: EF mapear a coleção privada _items
        builder.Metadata.FindNavigation(nameof(Order.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignorar eventos de domínio (não persistir)
        builder.Ignore(x => x.DomainEvents);
    }
}