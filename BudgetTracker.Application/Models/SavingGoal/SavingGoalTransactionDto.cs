using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models;

public class SavingGoalTransactionDto
(Guid id,Guid SavingGoalId,DateTime CreatedAt,Money Amount,string? Description);