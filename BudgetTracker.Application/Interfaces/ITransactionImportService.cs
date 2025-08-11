using BudgetTracker.Application.Models.Transaction;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionImportService
{
    Task<TransactionImportResult> ImportFromCsvAsync(Stream csvStream, Guid userId, CancellationToken cancellation);
}