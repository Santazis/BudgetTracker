using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Models.RecurringTransaction;

namespace BudgetTracker.Domain.Repositories;

public interface IRecurringTransactionRepository
{
    Task<List<RecurringTransaction>> GetByUserIdAsync(Guid userId,PaginationRequest request,CancellationToken cancellation);
    Task<RecurringTransaction> CreateAsync(RecurringTransaction transaction,CancellationToken cancellation);
    Task<RecurringTransaction?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    void Delete(RecurringTransaction transaction);
    Task AddRangeAsync(IEnumerable<RecurringTransaction> transactions,CancellationToken cancellation);
    void UpdateRange(List<RecurringTransaction> transactions);
    Task<List<RecurringTransaction>> GetByCursorAsync(Guid? lastId,DateTime? lastNextRun,int batchSize,DateTime now,DateTime end,CancellationToken cancellation);
    Task<List<RecurringTransaction>> GetByOffsetAsync(int offset,int batchSize,DateTime now,DateTime end,CancellationToken cancellation);
}