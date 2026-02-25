using MF.OrderManagement.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MF.OrderManagement.Infrastructure.Persistence.Configurations;

public sealed class PaymentConditionConfiguration : IEntityTypeConfiguration<PaymentCondition>
{
    public void Configure(EntityTypeBuilder<PaymentCondition> builder)
    {
        builder.ToTable("PaymentConditions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("PaymentConditionId");

        builder.Property(x => x.Description)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NumberOfInstallments)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}