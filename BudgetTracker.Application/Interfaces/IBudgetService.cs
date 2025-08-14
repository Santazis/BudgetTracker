using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Application.Models.Budget.Requests;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface IBudgetService
{
    Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync(Guid userId, CancellationToken cancellation);
    Task<BudgetDto> CreateBudgetAsync(CreateBudget request,Guid userId, CancellationToken cancellation);
    Task<BudgetDto> GetBudgetById(Guid id,Guid userId, CancellationToken cancellation);
    Task<BudgetDto> UpdateAsync(UpdateBudget request,Guid budgetId, Guid userId, CancellationToken cancellation);
    
}