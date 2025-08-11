namespace BudgetTracker.Application.Models.RecurringTransaction.Requests;

public record CreateRecurringTransaction(
    string? Description,
    Guid CategoryId,
    Guid? PaymentMethodId,
    decimal Amount,
    string Currency,
    string CronExpression,
    IEnumerable<Guid>? TagIds = null);