using BudgetTracker.Domain.Models.SavingGoal;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class SavingGoalRepository : ISavingGoalRepository
{
    private readonly ApplicationDbContext _context;

    public SavingGoalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(SavingGoal savingGoal, CancellationToken cancellation)
    {
        await _context.SavingGoals.AddAsync(savingGoal, cancellation);
    }

    public async Task<SavingGoal?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation)
    {
        return await _context.SavingGoals
            .Include(t=> t.Transactions)
            .FirstOrDefaultAsync(t=> t.Id == id && t.UserId == userId, cancellation);
    }
}