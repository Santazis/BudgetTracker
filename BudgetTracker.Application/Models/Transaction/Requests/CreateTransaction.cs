namespace BudgetTracker.Application.Models.Transaction.Requests;

public record CreateTransaction(string Name,Guid? PaymentMethodId,DateTime CreatedAt, decimal Amount, string Currency, string? Description,Guid CategoryId);
