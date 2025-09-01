using BudgetTracker.Domain.Common.Pagination;
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

    public void Delete(RecurringTransaction transaction)
    {
    }

    
    
    public async Task<List<RecurringTransaction>> GetByUserIdAsync(Guid userId,PaginationRequest request, CancellationToken cancellation)
    {
        var transactions = await _context.RecurringTransactions.AsNoTracking()
            .Where(t=> t.UserId == userId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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



    public async Task AddRangeAsync(IEnumerable<RecurringTransaction> transactions, CancellationToken cancellation)
    {
        await _context.RecurringTransactions.AddRangeAsync(transactions, cancellation);
    }

    public void UpdateRange(List<RecurringTransaction> transactions)
    {
        _context.RecurringTransactions.UpdateRange(transactions);
    }

    public async Task<List<RecurringTransaction>> GetByCursorAsync(Guid? lastId,DateTime? lastNextRun, int batchSize, DateTime now, DateTime end, CancellationToken cancellation)
    {
        var query = _context.RecurringTransactions.AsQueryable();
        if (lastId.HasValue && lastNextRun.HasValue) 
        {
            //Using tuple comparison for PostgresSql with indexed columns
            query = query.Where(t => EF.Functions.GreaterThan(
                ValueTuple.Create(t.Id,t.NextRun ),
                ValueTuple.Create(lastId,lastNextRun)) && t.NextRun >= now && t.NextRun < end);
        }
        else
        {
            query = query.Where(t => t.NextRun >= now);
        }
        
        var transactions = await query
            .AsNoTracking()
            .OrderBy(t=> t.Id)
            .ThenBy(t=> t.NextRun)
            .Take(batchSize)
            .Include(t=> t.RecurringTransactionTags)
            .ToListAsync(cancellation);
        return transactions;
    }

    public async Task<List<RecurringTransaction>> GetByOffsetAsync(int offset, int batchSize, DateTime now, DateTime end, CancellationToken cancellation)
    {
        var transactions = await _context.RecurringTransactions
            .AsNoTracking()
            .Where(t=> t.NextRun >= now)
            .Skip(offset)
            .Take(batchSize)
            .Include(t=> t.RecurringTransactionTags)
            .ToListAsync(cancellation);
        return transactions;
    }
}
