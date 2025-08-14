using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.Budget;

public record BudgetDto(
    Guid Id,
    string Name,
    string? Description,
    Money Limit,
    Money Spent,
    Money Remaining,
    bool IsExceeded,
    DateTime CreatedAt,
    DateTime PeriodStart,
    DateTime PeriodEnd
    )
{
    public static BudgetDto FromEntity(Domain.Models.Budget.Budget b,  
        Money spent, Money remaining) =>
        new BudgetDto(
            Id: b.Id,
            Name: b.Name,
            Description: b.Description,
            Limit: Money.Create(b.LimitAmount.Amount, b.LimitAmount.Currency),
            Spent: spent,
            Remaining: remaining,
            IsExceeded: spent > b.LimitAmount,
            CreatedAt: b.CreatedAt,
            PeriodStart: b.Period.PeriodStart,
            PeriodEnd: b.Period.PeriodEnd
        );
}