using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.RecurringTransaction.Requests;

public record UpdateRecurringTransaction(string? Description, string Currency,decimal Amount, string CronExpression);