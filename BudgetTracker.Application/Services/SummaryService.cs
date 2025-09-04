using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Redis;
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
    private readonly IRedisCacheService _redis;
    public SummaryService(ISummaryRepository summaryRepository, IRedisCacheService redis)
    {
        _summaryRepository = summaryRepository;
        _redis = redis;
    }

    public async Task<SummaryDto> GetSummaryAsync(Guid userId, TransactionFilter? filter,
        CancellationToken cancellation)
    {
        
        string key = $"summary-{userId}";
        var summaryCache = await _redis.GetStringAsync<SummaryDto>(key);
        if (summaryCache is not null)
        {
            return summaryCache;
        }
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
        await _redis.SetStringAsync(key, summary);
        return summary;
       
    }

    public async Task<IEnumerable<CategorySummary>> GetTopExpensesCategoriesInMonthAsync(Guid userId,
        CancellationToken cancellation)
    {
        string key = $"top-expenses-month-{userId}";
        var summaries = await _redis.GetStringAsync<IEnumerable<CategorySummary>>(key);
        if (summaries is not null)
        {
            return summaries;
        }
        var from = DateTime.UtcNow.AddMonths(-1);
        var to = DateTime.UtcNow;
        var result = await _summaryRepository.GetCategoriesTransactionsInMonthAsync(userId, from, to, cancellation);
        summaries = result.Select(r => new CategorySummary(
            new CategoryDto(r.Category.Name, r.Category.Id, r.Category.Type.ToString()),
            Money.Create(r.Amount, "USD"))).ToList();
        await _redis.SetStringAsync(key, summaries);
        return summaries;
    }


    public async Task<IEnumerable<CategoryExpenseComparisonDto>> GetMonthlySpendingComparisonAsync(Guid userId,
        CancellationToken cancellation)
    {
        string key = $"comparison-{userId}";
        var comparison = await _redis.GetStringAsync<IEnumerable<CategoryExpenseComparisonDto>>(key);
        if (comparison is not null)
        {
            return comparison;
        }
        var now = DateTime.UtcNow;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddTicks(-1);
        var previousMonthStart = currentMonthStart.AddMonths(-1);
        var categories =
            await _summaryRepository.GetMonthComparisonAsync(userId, previousMonthStart, currentMonthEnd, cancellation);

        comparison = categories.Where(c => c.CurrentMonth > 0 && c.PreviousMonth > 0)
            .Select(c => new CategoryExpenseComparisonDto(
                new CategoryDto(c.Category.Name, c.Category.Id, c.Category.Type.ToString()),
                c.CurrentMonth,
                c.PreviousMonth)).ToList();
        await _redis.SetStringAsync(key, comparison);
        return comparison;
    }
}