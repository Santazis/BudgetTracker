using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Filters;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Services;

public class SummaryService : ISummaryService
{
    private readonly ISummaryRepository _summaryRepository;

    public SummaryService(ISummaryRepository summaryRepository)
    {
        _summaryRepository = summaryRepository;
    }

    public async Task<SummaryDto> GetSummaryAsync(Guid userId, TransactionFilter? filter,
        CancellationToken cancellation)
    {
        var categoriesSpent = await _summaryRepository.GetSummaryAsync(userId,filter,cancellation);
        var totalIncome = categoriesSpent.Where(c=> c.Key.Type == CategoryTypes.Income).Sum(c=> c.Value);
        var totalExpense = categoriesSpent.Where(c=> c.Key.Type == CategoryTypes.Expense).Sum(c=> c.Value);
        var totalBalance = totalIncome - totalExpense;
        var byCategory = categoriesSpent.Select(c => new CategorySummary(
            new CategoryDto(c.Key.Name, c.Key.Id, c.Key.Type.ToString()),
            Money.Create(c.Value,"Usd")));
        var summary = new SummaryDto(
            Total: Money.Create(totalBalance,"USD"), 
            TotalIncome: Money.Create(totalIncome, "USD"), 
            TotalExpense: Money.Create(totalExpense,"USD"), 
            ByCategory: byCategory);
        return summary;
       
    }

    public async Task<IEnumerable<CategorySummary>> GetTopExpensesCategoriesInMonthAsync(Guid userId,
        CancellationToken cancellation)
    {
        var from = DateTime.UtcNow.AddMonths(-1);
        var to = DateTime.UtcNow;
        var result = await _summaryRepository.GetCategoriesTransactionsInMonthAsync(userId, from, to, cancellation);
        var summaries = result.Select(r => new CategorySummary(
            new CategoryDto(r.Category.Name, r.Category.Id, r.Category.Type.ToString()),
            Money.Create(r.Amount, "USD")));

        return summaries;
    }


    public async Task<IEnumerable<CategoryExpenseComparisonDto>> GetMonthlySpendingComparisonAsync(Guid userId,
        CancellationToken cancellation)
    {
        var now = DateTime.UtcNow;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddTicks(-1);
        var previousMonthStart = currentMonthStart.AddMonths(-1);
        var categories =
            await _summaryRepository.GetMonthComparisonAsync(userId, previousMonthStart, currentMonthEnd, cancellation);

        return categories.Where(c => c.CurrentMonth > 0 && c.PreviousMonth > 0)
            .Select(c => new CategoryExpenseComparisonDto(
                new CategoryDto(c.Category.Name, c.Category.Id, c.Category.Type.ToString()),
                c.CurrentMonth,
                c.PreviousMonth));
    }
}