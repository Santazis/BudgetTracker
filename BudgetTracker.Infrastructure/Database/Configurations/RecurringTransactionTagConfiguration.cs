using BudgetTracker.Domain.Models.RecurringTransaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class RecurringTransactionTagConfiguration : IEntityTypeConfiguration<RecurringTransactionTag>
{
    public void Configure(EntityTypeBuilder<RecurringTransactionTag> builder)
    {
        builder.ToTable(nameof(RecurringTransactionTag), "public")
            .HasKey(t => new { t.RecurringTransactionId, t.TagId });
        
        builder.HasOne(t=> t.RecurringTransaction)
            .WithMany(t=> t.RecurringTransactionTags)
            .HasForeignKey(t=> t.RecurringTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t=> t.Tag)
            .WithMany()
            .HasForeignKey(t=> t.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}