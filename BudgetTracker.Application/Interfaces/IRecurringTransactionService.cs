using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;
using BudgetTracker.Domain.Common.Pagination;

namespace BudgetTracker.Application.Interfaces;

public interface IRecurringTransactionService
{
    Task<IEnumerable<RecurringTransactionDto>> GetByUserIdAsync(Guid userId,PaginationRequest request,CancellationToken cancellation);
    Task CreateRecurringTransactionAsync(CreateRecurringTransaction request, Guid userId,
        CancellationToken cancellation);
    Task CreateTransactionFromRecurringTransactionAsync(Guid recurringTransactionId, Guid userId,
        CancellationToken cancellation);
    Task DeleteAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation);
    Task UpdateAsync(Guid recurringTransactionId, Guid userId,UpdateRecurringTransaction request, CancellationToken cancellation);
    Task ActivateAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation);
    Task DeactivateAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation);
}