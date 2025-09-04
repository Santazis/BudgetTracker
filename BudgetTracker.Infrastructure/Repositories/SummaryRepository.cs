using BudgetTracker.Application.Extensions.Filters;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;
using BudgetTracker.Domain.Repositories.Models;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class SummaryRepository : ISummaryRepository
{
    private readonly ApplicationDbContext _context;

    public SummaryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<CategoryTotalAmount>> GetCategoriesTransactionsInMonthAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellation)
    {
        // var categories =  _context.Transactions.AsNoTracking()
        //     .Where(t => t.UserId == userId && t.Category.Type == CategoryTypes.Expense)
        //     .Where(t => t.CreatedAt >= from && t.CreatedAt <= to)
        //     .GroupBy(t => t.Category)
        //     .Select(g => new
        //     {
        //         Category = g.Key,
        //         Amount = g.Sum(t => t.Amount.Amount)
        //     })
        //     .OrderByDescending(g=> g.Amount)
        //     .Take(1).Select(g=> g.Category.Id);
        // // var transactions = await _context.Transactions.AsNoTracking()
        // //     .Where(t => t.UserId == userId && t.Category.Type == CategoryTypes.Expense)
        // //     .Where(t => t.CreatedAt >= from && t.CreatedAt <= to)
        // //     .Include(t => t.Category)
        // //     .ToListAsync(cancellation);
        // return transactions;
        var result =  _context.Transactions.AsNoTracking()
            
            .Where(t => t.UserId == userId && t.Category.Type == CategoryTypes.Expense)
            .Where(t => t.CreatedAt >= from && t.CreatedAt <= to)
            .GroupBy(t => t.Category)
            .Select(g => new
            {
                Category = g.Key,
                Amount = g.Sum(t => t.Amount.Amount)
            })
            .OrderByDescending(g=> g.Amount)
            .Take(5).Select(g=>new CategoryTotalAmount(g.Category,g.Amount))
            .ToListAsync(cancellation);
        return result;
    }

    public async  Task<List<CategoryMonthlyComparison>> GetMonthComparisonAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellation)
    {
        var categories = await _context.Transactions.AsNoTracking()
            .Where(t => t.UserId == userId)
            .Where(t => t.CreatedAt >= from && t.CreatedAt <= to)
            .GroupBy(t => t.Category.Id)
            .Select(g => new CategoryMonthlyComparison(g.First().Category,
                g.Where(t=> t.CreatedAt.Month == from.Month).Sum(t=> t.Amount.Amount),
                g.Where(t=> t.CreatedAt.Month == to.Month).Sum(t=> t.Amount.Amount)))
            .ToListAsync(cancellation);
        return categories;
    }

    public async Task<Dictionary<Category,decimal>> GetSummaryAsync(Guid userId, TransactionFilter? filter,
        CancellationToken cancellation)
    {
        var result = await _context.Transactions.AsNoTracking()
            .Where(t=> t.UserId == userId)
            .Include(t=> t.Category)
            .Filter(filter)
            .GroupBy(t=> t.Category)
            .Select(g=> new {g.Key,Amount = g.Sum(t=> t.Amount.Amount)})
            .ToDictionaryAsync(g=> g.Key, g=> g.Amount, cancellation);
        return result;
    }
    
}
