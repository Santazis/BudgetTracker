using BudgetTracker.Application.Interfaces;
using BudgetTracker.Infrastructure.Database;

namespace BudgetTracker.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellation)
    {
        return _context.SaveChangesAsync(cancellation);
    }

    public Task BeginTransactionAsync(CancellationToken cancellation)
    {
        return _context.Database.BeginTransactionAsync(cancellation);
    }

    public Task CommitTransactionAsync(CancellationToken cancellation)
    {
        return _context.Database.CommitTransactionAsync(cancellation);
    }

    public Task RollbackTransactionAsync(CancellationToken cancellation)
    {
        return _context.Database.RollbackTransactionAsync(cancellation);
    }
}