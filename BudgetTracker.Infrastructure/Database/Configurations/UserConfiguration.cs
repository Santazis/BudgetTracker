using BudgetTracker.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), "public")
            .HasKey(u => u.Id);

        builder.ComplexProperty(u => u.Name, b =>
        {
            b.Property(n => n.Firstname).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
            b.Property(n => n.Lastname).HasColumnName("LastName").HasMaxLength(100).IsRequired();
        });
        builder.Property(u => u.PasswordHash)
            .HasColumnName("PasswordHash")
            .HasMaxLength(124)
            .IsRequired();
        builder.Property(u => u.CreatedAt)
            .HasColumnName("CreatedAt");

        builder.Property(u => u.Email)
            .HasConversion(e => e.Value,
                value => new Email(value)).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.PaymentMethods)
            .WithOne(p=> p.User)
            .HasForeignKey(u=> u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}