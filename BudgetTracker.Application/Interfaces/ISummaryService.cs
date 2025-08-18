using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Filters;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface ISummaryService
{
    Task<SummaryDto> GetSummaryAsync(Guid userId,TransactionFilter? filter, CancellationToken cancellation);
    Task<IEnumerable<CategorySummary>> GetTopExpensesCategoryInMonthAsync(Guid userId,CancellationToken cancellation);    
}