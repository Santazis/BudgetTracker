using BudgetTracker.Domain.Models.SavingGoal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class SavingGoalTransactionConfiguration : IEntityTypeConfiguration<SavingGoalTransaction>
{
    public void Configure(EntityTypeBuilder<SavingGoalTransaction> builder)
    {
        builder.ToTable(nameof(SavingGoalTransaction), "public")
            .HasKey(t => t.Id);
        
        builder.HasOne(t=> t.SavingGoal)
            .WithMany(t=> t.Transactions)
            .HasForeignKey(t=> t.SavingGoalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.Description).HasMaxLength(256);

        builder.ComplexProperty(t => t.Amount, b =>
        {
            b.Property(t=> t.Amount).HasColumnName("Amount").IsRequired();
            b.Property(t=> t.Currency).HasColumnName("Currency").IsRequired();
        });
    }
}