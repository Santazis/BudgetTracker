using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models;

public record SavingGoalDto(
    Guid Id,
    Guid UserId,
    DateTime PeriodStart,
    DateTime PeriodEnd,
    Money TargetAmount,
    Money CurrentAmount,
    Money AmountLeft,
    IEnumerable<SavingGoalTransactionDto> Transactions)
{
    public static SavingGoalDto FromEntity(Domain.Models.SavingGoal.SavingGoal g)
    {
        return new SavingGoalDto(
            g.Id,
            g.UserId,
            g.Period.PeriodStart,
            g.Period.PeriodEnd,
            g.TargetAmount,
            g.CurrentAmount,
            g.AmountLeft,
            g.Transactions.Select(t =>
                new SavingGoalTransactionDto(t.Id, t.SavingGoalId, t.CreatedAt, t.Amount, t.Description)));
    }
}