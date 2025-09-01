using BudgetTracker.Domain.Models.SavingGoal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class SavingGoalConfiguration : IEntityTypeConfiguration<SavingGoal>
{
    public void Configure(EntityTypeBuilder<SavingGoal> builder)
    {
        builder.ToTable(nameof(SavingGoal), "public")
            .HasKey(g => g.Id);

        builder.ComplexProperty(t => t.TargetAmount, b =>
        {
            b.Property(t => t.Amount).HasColumnName("Amount").IsRequired();
            b.Property(t => t.Currency).HasColumnName("Currency").IsRequired();
        });
        builder.ComplexProperty(t=> t.Period, p =>
        {
            p.Property(p => p.PeriodStart).HasColumnName("PeriodStart").IsRequired();
            p.Property(p => p.PeriodEnd).HasColumnName("PeriodEnd").IsRequired();
        });
        builder.Property(t => t.Name).HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(256);
        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
        builder.HasMany(t => t.Transactions)
            .WithOne(t => t.SavingGoal)
            .HasForeignKey(t => t.SavingGoalId)
            .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
    }
}