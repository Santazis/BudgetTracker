using BudgetTracker.Application.Services;

namespace BudgetTracker.Application.Interfaces;

public interface IRecurringTransactionProcessingService
{
    Task<RecurringTransactionProcessingService.ProcessingResult> ProcessRecurringTransactionsByCursorPaginationAsync(CancellationToken cancellation);
}