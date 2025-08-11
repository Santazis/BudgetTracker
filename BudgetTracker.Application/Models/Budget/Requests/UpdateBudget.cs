namespace BudgetTracker.Application.Models.Budget.Requests;

public record UpdateBudget(
    decimal LimitAmount,
    string Currency,
    string? Description,
    string Name,
    DateTime PeriodStart,
    DateTime PeriodEnd
);