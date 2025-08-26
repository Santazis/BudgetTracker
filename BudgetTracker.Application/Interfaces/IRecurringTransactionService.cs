using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Pagination;

namespace BudgetTracker.Application.Interfaces;

public interface IRecurringTransactionService
{
    Task<IEnumerable<RecurringTransactionDto>> GetByUserIdAsync(Guid userId,PaginationRequest request,CancellationToken cancellation);
    Task CreateRecurringTransactionAsync(CreateRecurringTransaction request, Guid userId,
        CancellationToken cancellation);
    Task<Result> CreateTransactionFromRecurringTransactionAsync(Guid recurringTransactionId, Guid userId,
        CancellationToken cancellation);
    Task<Result> DeleteAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation);
    Task<Result> UpdateAsync(Guid recurringTransactionId, Guid userId,UpdateRecurringTransaction request, CancellationToken cancellation);
    Task<Result> ActivateAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation);
    Task<Result> DeactivateAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation);
}