using BudgetTracker.Domain.Models.RecurringTransaction;

namespace BudgetTracker.Domain.Repositories;

public interface IRecurringTransactionRepository
{
    Task<List<RecurringTransaction>> GetByUserIdAsync(Guid userId,CancellationToken cancellation);
    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction,CancellationToken cancellation);
    Task<RecurringTransaction?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    IAsyncEnumerable<RecurringTransaction> GetAsAsync(Guid userId);
    Task AddRangeAsync(IEnumerable<RecurringTransaction> transactions,CancellationToken cancellation);
    Task<List<RecurringTransaction>> GetUpcomingAsList(CancellationToken cancellation);
    IAsyncEnumerable<RecurringTransaction> GetUpcomingAsAsync(CancellationToken cancellation);
}