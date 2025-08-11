using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Filters;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Services;

public class SummaryService : ISummaryService
{
    private readonly ITransactionRepository _transactionRepository;

    public SummaryService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<SummaryDto> GetSummaryAsync(Guid userId, TransactionFilter? filter,
        CancellationToken cancellation)
    {
        var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId, filter, cancellation);
        var transactionsGroupedByCategory = transactions.GroupBy(t => t.CategoryId);

        var categorySummary = transactionsGroupedByCategory.Select(g => new CategorySummary(
            TotalAmount: Money.Create(g.Sum(t => t.Amount.Amount), g.First().Amount.Currency),
            Category: new CategoryDto(g.First().Category.Name, g.First().Category.Id,
                g.First().Category.Type.ToString())));

        var totalIncome = transactions.Where(t => t.Category.Type == CategoryTypes.Income)
            .Select(t => t.Amount)
            .DefaultIfEmpty(Money.Create(0,"USD"))
            .Aggregate((a, b) => a + b);
        var totalExpense = transactions.Where(t => t.Category.Type == CategoryTypes.Expense)
            .Select(t => t.Amount)
            .DefaultIfEmpty(Money.Create(0,"USD"))
            .Aggregate((a, b) => a + b);
        var totalBalance = totalIncome - totalExpense;
        var summary = new SummaryDto(
            Total: totalBalance,
            TotalIncome: totalIncome,
            TotalExpense: totalExpense,
            ByCategory: categorySummary.OrderBy(c => c.Category.Type).ToList());
        return summary;
    }
}