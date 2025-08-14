using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Models.Budget;

namespace BudgetTracker.Domain.Repositories;

public interface IBudgetRepository
{
    Task<Budget> AddAsync(Budget budget,CancellationToken cancellation);
    Task<List<Budget>> GetByUserIdAsync(Guid userId,PaginationRequest request,CancellationToken cancellation);
    Task<List<Budget>> GetActiveBudgetsByUserIdAsync(Guid userId,CancellationToken cancellation);
    Task<Budget?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    void Delete(Budget budget);
    Task<int> CountAsync(Guid userId,CancellationToken cancellation);
}