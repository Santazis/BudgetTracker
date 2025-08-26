using BudgetTracker.Application.Interfaces.Analytics;
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

    public BudgetAnalyticService(IBudgetRepository budgetRepository, ITransactionRepository transactionRepository)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<BudgetForecast>> GetForecastAsync(Guid userId, Guid budgetId,
        CancellationToken cancellation)
    {
        var budget = await _budgetRepository.GetByIdAsync(budgetId, userId, cancellation);
        if (budget is null) return Result<BudgetForecast>.Failure(BudgetErrors.BudgetNotFound);
        TransactionFilter filter = new()
        {
            Categories = [budget.CategoryId],
            From = budget.Period.PeriodStart,
            To = budget.Period.PeriodEnd,
        };
        var spent = await _transactionRepository.GetSpentAmountAsync(userId, filter, cancellation);
        var days = (int)( DateTime.UtcNow - budget.Period.PeriodStart ).TotalDays;
        var remainingDays = (int)(budget.Period.PeriodEnd - DateTime.UtcNow).TotalDays;
        var dailyAverage = remainingDays > 0 ? spent / days : spent;
        var remainingAmount = budget.LimitAmount.Amount - spent;
        var willExceed = remainingAmount < dailyAverage * remainingDays;
        if (willExceed)
        {
            var targetAverage = remainingAmount / remainingDays - 1;
            return Result<BudgetForecast>.Success(new BudgetForecast(
                BudgetDto.FromEntity(budget, Money.Create(spent, budget.LimitAmount.Currency),
                    Money.Create(remainingAmount, budget.LimitAmount.Currency)),
                remainingDays, dailyAverage,targetAverage));
        }
        return Result<BudgetForecast>.Success(new BudgetForecast(
            BudgetDto.FromEntity(budget, Money.Create(spent, budget.LimitAmount.Currency),
                Money.Create(remainingAmount, budget.LimitAmount.Currency)),
            remainingDays, dailyAverage));
    }
}