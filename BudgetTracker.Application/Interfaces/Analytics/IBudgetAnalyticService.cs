using BudgetTracker.Application.Models.Analytics;
using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces.Analytics;

public interface IBudgetAnalyticService
{
    Task<Result<BudgetForecast>> GetForecastAsync(Guid userId,Guid budgetId,CancellationToken cancellation);
}