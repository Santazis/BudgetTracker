using BudgetTracker.Domain.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class TransactionTagConfiguration : IEntityTypeConfiguration<TransactionTag>
{
    public void Configure(EntityTypeBuilder<TransactionTag> builder)
    {
        builder.ToTable(nameof(TransactionTag), "public")
            .HasKey(t => new { t.TransactionId, t.TagId });
        
        builder.HasOne(t=> t.Transaction)
            .WithMany(t=> t.TransactionTags)
            .HasForeignKey(t=> t.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t=> t.Tag)
            .WithMany()
            .HasForeignKey(t=> t.TagId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}