namespace BudgetTracker.Application.Models.RecurringTransaction.Requests;

public record CreateRecurringTransaction(
    string? Description,
    Guid CategoryId,
    Guid? PaymentMethodId,
    decimal Amount,
    string Currency,
    string CronExpression,
    DateTime RunDate,
    IEnumerable<Guid>? TagIds = null);