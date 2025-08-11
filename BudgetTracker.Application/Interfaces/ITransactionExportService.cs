using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionExportService
{
    Task<byte[]> ExportToCsvAsync(Guid userId,TransactionFilter filter, CancellationToken cancellation);
}