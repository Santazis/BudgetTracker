using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.SavingGoal.Requests;

public record CreateSavingGoal(
    Money TargetAmount,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    string Name,
    string? Description);