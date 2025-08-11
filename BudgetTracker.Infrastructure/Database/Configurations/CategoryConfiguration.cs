using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(Category), "public")
            .HasKey(c => c.Id);

        builder.Property(c => c.Type)
            .HasConversion<string>();
        builder.HasOne(c=> c.User)
            .WithMany()
            .HasForeignKey(c=> c.UserId)
            .OnDelete( DeleteBehavior.Cascade);
    }
}