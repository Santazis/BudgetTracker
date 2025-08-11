
using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.RecurringTransaction;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Models.User;
using BudgetTracker.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TransactionTag> TransactionTags { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
    public DbSet<RecurringTransactionTag> RecurringTransactionTags { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionTagConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringTransactionConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringTransactionTagConfiguration());
    }
}