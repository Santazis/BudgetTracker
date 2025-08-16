using BudgetTracker.Domain.Models.RecurringTransaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.ToTable(nameof(RecurringTransaction), "public")
            .HasKey(t => t.Id);

        builder.Property(t => t.Description).HasMaxLength(255);
        builder.HasIndex(t => t.NextRun);
        builder.ComplexProperty(t => t.Amount, t =>
        {
            t.Property(t => t.Amount).HasColumnName("Amount").IsRequired();
            t.Property(t => t.Currency).HasColumnName("Currency").IsRequired();
        });

        builder.HasIndex(t => t.UserId);

        builder.HasIndex(t => t.CategoryId);

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t=> t.PaymentMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(t=> t.RecurringTransactionTags)
            .WithOne(t=> t.RecurringTransaction)
            .HasForeignKey(t=> t.RecurringTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
}