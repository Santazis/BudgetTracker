using System.Linq.Expressions;
using BudgetTracker.Application.Extensions.Filters;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter? filter,PaginationRequest request, CancellationToken cancellation)
    {
        var transactions = await _context.Transactions.AsNoTracking()
            .OrderByDescending(t=> t.CreatedAt)
            .Where(t=> t.UserId == userId)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Filter(filter)
            .Include(t=> t.Category)
            .Include(t=> t.PaymentMethod)
            .Include(t=> t.TransactionTags)
            .ThenInclude(t=> t.Tag)
            .ToListAsync(cancellation);
        return transactions;
    }

    public async Task<Transaction> CreateAsync(Transaction transaction, CancellationToken cancellation)
    {
        await _context.Transactions.AddAsync(transaction, cancellation);
        return transaction;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation)
    {
        var transaction = await _context.Transactions
            .Include(t=> t.Category)
            .Include(t=> t.PaymentMethod)
            .Include(t=> t.TransactionTags)
            .ThenInclude(t=> t.Tag)
            .FirstOrDefaultAsync(t => t.Id == id, cancellation);
        return transaction;
    }

    public void Delete(Transaction transaction)
    {
        _context.Transactions.Remove(transaction);
    }
    
    public async Task CreateRangeAsync(IEnumerable<Transaction> transactions, CancellationToken cancellation)
    {
        await _context.Transactions.AddRangeAsync(transactions, cancellation);
    }

    public IAsyncEnumerable<Transaction> GetTransactionsAsync(TransactionFilter? filter, Guid userId,
        CancellationToken cancellation)
    {
        return  _context.Transactions.AsNoTracking().Where(t => t.UserId == userId)
            .Filter(filter)
            .Include(t=> t.Category)
            .Include(t=> t.PaymentMethod)
            .Include(t=> t.TransactionTags)
            .ThenInclude(t=> t.Tag)
            .AsAsyncEnumerable();
    }

    public async Task<int> CountAsync(Guid userId, CancellationToken cancellation)
    {
        var count = await _context.Transactions.Where(t => t.UserId == userId).CountAsync(cancellation);
        return count;
    }

    public async Task<decimal> GetSpentAmountAsync(Guid userId, TransactionFilter? filter, CancellationToken cancellation)
    {
        var amount = await _context.Transactions.AsNoTracking().Where(t=> t.UserId == userId)
            .Filter(filter)
            .SumAsync(t=> t.Amount.Amount, cancellation);
        return amount;
    }

    public async Task<List<Transaction>> GetAllAsync(Guid userId, TransactionFilter? filter, CancellationToken cancellation)
    {
        var transactions =await _context.Transactions.AsNoTracking().Where(t => t.UserId == userId)
            .Filter(filter).ToListAsync(cancellation);
        return transactions;
    }
}