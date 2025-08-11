namespace BudgetTracker.Application.Models.Transaction;

public record ParsedTransactionResult(
    string Name,
    string? Description,
    DateTime Date,
    decimal Amount,
    Guid? CategoryId,
    Guid? PaymentMethodId);