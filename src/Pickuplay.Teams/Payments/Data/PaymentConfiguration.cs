using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Pickuplay.Teams.Payments.Models;

namespace Pickuplay.Teams.Payments.Data;


public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ItemType).HasMaxLength(50).IsRequired();
        builder.Property(p => p.ItemId).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Currency).HasMaxLength(3).IsRequired();
        builder.Property(p => p.StripePaymentIntentId).HasMaxLength(100).IsRequired();
        builder.Property(p => p.FailureReason).HasMaxLength(500);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(p => p.StripePaymentIntentId).IsUnique();
        builder.HasIndex(p => new { p.ItemType, p.ItemId });
        builder.HasIndex(p => p.UserId);
    }
}