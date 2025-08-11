using BudgetTracker.Application.Models.Category;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.Transaction;

public record BudgetTransactionDto(
    Money Amount,
    string? Description,
    DateTime CreatedAt,
    Guid TransactionId);