using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;

namespace BudgetTracker.Application.Interfaces;

public interface IRecurringTransactionService
{
    Task<IEnumerable<RecurringTransactionDto>> GetRecurringTransactionsAsync(Guid userId,CancellationToken cancellation);

    Task CreateRecurringTransactionAsync(CreateRecurringTransaction request, Guid userId,
        CancellationToken cancellation);
    
    Task CreateTransactionFromRecurringTransactionAsync(Guid recurringTransactionId, Guid userId,
        CancellationToken cancellation);
}