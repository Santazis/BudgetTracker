using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionImportService
{
    Task<TransactionImportResult> ImportFromCsvAsync(Stream csvStream, Guid userId,ImportTransactionRequest request, CancellationToken cancellation);
}