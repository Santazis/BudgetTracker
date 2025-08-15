using BudgetTracker.Domain.Common.Pagination;
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

    public async Task<List<Budget>> GetByUserIdAsync(Guid userId,PaginationRequest request, CancellationToken cancellation)
    {
        var budgets = await _context.Budgets.Where(b => b.UserId == userId).AsNoTracking()
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellation);
        return budgets;
    }

    public void Delete(Budget budget)
    {
        _context.Budgets.Remove(budget);
    }

    public async Task<List<Budget>> GetActiveBudgetsByUserIdAsync(Guid userId, CancellationToken cancellation)
    {
        var budgets = await _context.Budgets.AsNoTracking()
            .Where(b=> b.UserId == userId && b.Period.PeriodEnd.Date >= DateTime.UtcNow.Date)
            .ToListAsync(cancellation);
        return budgets;
    }


    public Task<Budget?> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation)
    {
        var budget = _context.Budgets.FirstOrDefaultAsync(b => b.Id == id  && b.UserId == userId, cancellation);
        return budget;
    }

    public async Task<int> CountAsync(Guid userId, CancellationToken cancellation)
    {
        var count = await _context.Budgets.Where(b => b.UserId == userId).CountAsync(cancellation);
        return count;
    }
}