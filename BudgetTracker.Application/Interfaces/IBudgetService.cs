using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Application.Models.Budget.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface IBudgetService
{
    Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync(Guid userId, CancellationToken cancellation);
    Task<Result<BudgetDto>> CreateBudgetAsync(CreateBudget request,Guid userId, CancellationToken cancellation);
    Task<Result<BudgetDto>> GetBudgetById(Guid id,Guid userId, CancellationToken cancellation);
    Task<Result<BudgetDto>> UpdateAsync(UpdateBudget request,Guid budgetId, Guid userId, CancellationToken cancellation);
    Task<Result> DeleteAsync(Guid budgetId, Guid userId, CancellationToken cancellation);
}