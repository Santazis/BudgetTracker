using BudgetTracker.Domain.Models.RecurringTransaction;

namespace BudgetTracker.Domain.Repositories;

public interface IRecurringTransactionRepository
{
    Task<List<RecurringTransaction>> GetByUserIdAsync(Guid userId,CancellationToken cancellation);
    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction,CancellationToken cancellation);
    Task<RecurringTransaction?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
}