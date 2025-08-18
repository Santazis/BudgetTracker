namespace BudgetTracker.Application.Models.Transaction.Requests;

public record CreateTransaction(Guid? PaymentMethodId,DateTime CreatedAt, decimal Amount, string Currency, string? Description,Guid CategoryId);
