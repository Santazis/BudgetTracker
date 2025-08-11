using BudgetTracker.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken), "public")
            .HasKey(t => t.Id);

        builder.HasIndex(t => t.UserId);
        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);
        builder.Property(t=> t.Revoked)
            .HasColumnName("Revoked");

        builder.Property(t => t.Token)
            .IsRequired();
    }
}