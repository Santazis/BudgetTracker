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
}