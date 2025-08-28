using BudgetTracker.Application.Models.Budget;

namespace BudgetTracker.Application.Models.Analytics;

public record BudgetForecast(
    BudgetDto Budget,
    int RemainingDays,
    decimal DailyAverage,
    decimal AverageDailyLimit = 0
);
