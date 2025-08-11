using BudgetTracker.Domain.Models.RecurringTransaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public RecurringTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecurringTransaction>> GetByUserIdAsync(Guid userId, CancellationToken cancellation)
    {
        var transactions = await _context.RecurringTransactions.AsNoTracking()
            .Where(t=> t.UserId == userId)
            .Include(t=> t.PaymentMethod)
            .Include(t=> t.Category)
            .Include(t=> t.RecurringTransactionTags)
            .ToListAsync(cancellation);
        return transactions;
    }

    public async Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction, CancellationToken cancellation)
    {
        await _context.RecurringTransactions.AddAsync(transaction, cancellation);
        return transaction;
    }

    public async Task<RecurringTransaction?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation)
    {
        var transaction = await _context.RecurringTransactions
            .Include(t=> t.PaymentMethod)
            .Include(t=> t.RecurringTransactionTags)
            .Include(t=> t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellation);
        return transaction;
    }
}