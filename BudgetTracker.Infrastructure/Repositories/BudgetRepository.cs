using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly ApplicationDbContext _context;

    public BudgetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Budget> AddAsync(Budget budget, CancellationToken cancellation)
    {
        await _context.Budgets.AddAsync(budget, cancellation);
        return budget;
    }

    public Task<Budget?> GetByUserIdAsync(Guid userId, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    public void Delete(Budget budget)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Budget>> GetActiveBudgetsByUserIdAsync(Guid userId, CancellationToken cancellation)
    {
        var budgets = await _context.Budgets.AsNoTracking()
            .Where(b=> b.UserId == userId && b.Period.PeriodEnd >= DateTime.UtcNow)
            .ToListAsync(cancellation);
        
        return budgets;
    }


    public Task<Budget?> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation)
    {
        var budget = _context.Budgets.FirstOrDefaultAsync(b => b.Id == id  && b.UserId == userId, cancellation);
        return budget;
    }
}