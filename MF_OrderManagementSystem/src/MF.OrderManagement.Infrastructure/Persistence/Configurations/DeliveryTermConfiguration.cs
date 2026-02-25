using MF.OrderManagement.Domain.Entities.Deliveries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MF.OrderManagement.Infrastructure.Persistence.Configurations;

public sealed class DeliveryTermConfiguration : IEntityTypeConfiguration<DeliveryTerms>
{
    public void Configure(EntityTypeBuilder<DeliveryTerms> builder)
    {
        builder.ToTable("DeliveryTerms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("DeliveryTermId")
            .IsRequired();

        builder.Property(x => x.OrderId)
            .IsRequired();

        builder.Property(x => x.EstimatedDeliveryDate)
            .IsRequired();

        builder.Property(x => x.DeliveryDays)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.OrderId).IsUnique(); // 1-para-1 com Order
    }
}