using BudgetTracker.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable(nameof(PaymentMethod), "public")
            .HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Type).HasConversion<string>();
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Details).HasMaxLength(100);
        builder.HasOne(p => p.User)
            .WithMany(u=> u.PaymentMethods)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}