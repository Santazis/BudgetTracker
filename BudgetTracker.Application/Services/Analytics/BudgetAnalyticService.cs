using BudgetTracker.Application.Interfaces.Analytics;
using BudgetTracker.Application.Interfaces.Redis;
using BudgetTracker.Application.Models.Analytics;
using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Services.Analytics;

public class BudgetAnalyticService : IBudgetAnalyticService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IRedisCacheService _redis;
    public BudgetAnalyticService(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository, IRedisCacheService redis)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
        _redis = redis;
    }

    public async Task<Result<BudgetForecast>> GetForecastAsync(Guid userId, Guid budgetId,
        CancellationToken cancellation)
    {
        string key = $"budget-forecast-{userId}-{budgetId}";
        var forecast = await _redis.GetStringAsync<BudgetForecast>(key);
        if (forecast is not null)
        {
            return Result<BudgetForecast>.Success(forecast);
        }
        var budget = await _budgetRepository.GetByIdAsync(budgetId, userId, cancellation);
        if (budget is null) return Result<BudgetForecast>.Failure(BudgetErrors.BudgetNotFound);
        TransactionFilter filter = new()
        {
            Categories = [budget.CategoryId],
            From = budget.Period.PeriodStart,
            To = budget.Period.PeriodEnd,
        };
        var spent = await _transactionRepository.GetSpentAmountAsync(userId, filter, cancellation);
        var daysPast = (int)( DateTime.UtcNow - budget.Period.PeriodStart ).TotalDays;
        daysPast = daysPast > 0 ? daysPast : 1;
        var remainingDays = (int)(budget.Period.PeriodEnd - DateTime.UtcNow).TotalDays;
        var dailyAverage = remainingDays > 0 ? spent / daysPast : spent;
        var remainingAmount = budget.LimitAmount.Amount - spent;
        var willExceed = remainingAmount < dailyAverage * remainingDays;
        if (willExceed)
        {
            var targetAverage = remainingAmount / remainingDays - 1;
            forecast = new BudgetForecast(
                BudgetDto.FromEntity(budget, Money.Create(spent, budget.LimitAmount.Currency),
                    Money.Create(remainingAmount, budget.LimitAmount.Currency)),
                remainingDays, dailyAverage,targetAverage);
            await _redis.SetStringAsync(key, forecast);
            return Result<BudgetForecast>.Success(forecast);
        }
        forecast = new BudgetForecast(
            BudgetDto.FromEntity(budget, Money.Create(spent, budget.LimitAmount.Currency),
                Money.Create(remainingAmount, budget.LimitAmount.Currency)),
            remainingDays, dailyAverage);
        await _redis.SetStringAsync(key, forecast);
        return Result<BudgetForecast>.Success(forecast);
    }
}