using BudgetTracker.Domain.Models.Budget;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable(nameof(Budget), "public")
            .HasKey(b => b.Id);
        builder.HasIndex(b => b.UserId);
        builder.HasIndex(b => b.CategoryId);
        
        builder.HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b=> b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ComplexProperty(b => b.LimitAmount, a =>
        {
            a.Property(a => a.Amount).HasColumnName("Amount").IsRequired();
            a.Property(a => a.Currency).HasColumnName("Currency").IsRequired();
        });
        builder.Property(b => b.Name).HasMaxLength(100);
        builder.Property(b => b.Description).HasMaxLength(256);
        builder.ComplexProperty(b => b.Period, p =>
        {
            p.Property(p => p.PeriodStart).HasColumnName("PeriodStart").IsRequired();
            p.Property(p => p.PeriodEnd).HasColumnName("PeriodEnd").IsRequired();
        });
    }
}