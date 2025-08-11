using BudgetTracker.Domain.Models.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(nameof(Transaction), "public")
            .HasKey(t => t.Id);

        builder.ComplexProperty(t => t.Amount, m =>
        {
            m.Property(m => m.Amount).HasColumnName("Amount").IsRequired();
            m.Property(m => m.Currency).HasColumnName("Currency").IsRequired();
        });
        builder.Property(t => t.CreatedAt)
            .HasColumnName("CreatedAt");

        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(t => t.Description).HasMaxLength(255);
        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(t => t.UserId);

        builder.HasOne(t => t.PaymentMethod)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(t=> t.TransactionTags)
            .WithOne(t=> t.Transaction)
            .HasForeignKey(t=> t.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}