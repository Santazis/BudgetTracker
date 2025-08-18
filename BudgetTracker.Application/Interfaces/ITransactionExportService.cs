using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionExportService
{
    Task ExportToCsvAsync(Guid userId,TransactionFilter filter,Stream output, CancellationToken cancellation);
}