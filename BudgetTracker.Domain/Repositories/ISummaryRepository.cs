using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories.Models;

namespace BudgetTracker.Domain.Repositories;

public interface ISummaryRepository
{
    Task<List<CategoryTotalAmount>> GetCategoriesTransactionsInMonthAsync(Guid userId,DateTime from,DateTime to,CancellationToken cancellation);
    Task<List<CategoryMonthlyComparison>> GetMonthComparisonAsync(Guid userId,DateTime from,DateTime to,CancellationToken cancellation);
}