using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Filters;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface ISummaryService
{
    Task<SummaryDto> GetSummaryAsync(Guid userId,TransactionFilter? filter, CancellationToken cancellation);
}