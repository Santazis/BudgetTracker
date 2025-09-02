using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.SavingGoal.Requests;

public record CreateSavingGoal(
    decimal TargetAmount,
    string Currency,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    string Name,
    string? Description);