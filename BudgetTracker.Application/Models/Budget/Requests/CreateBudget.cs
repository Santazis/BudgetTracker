using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.Budget.Requests;

public record CreateBudget
(
    Guid CategoryId,
    string? Description,
    string Name,
    string Currency,
    decimal LimitAmount,
    DateTime PeriodStart,
    DateTime PeriodEnd);