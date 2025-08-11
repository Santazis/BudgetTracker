using BudgetTracker.Domain.Models.Category;

namespace BudgetTracker.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category> CreateAsync(Category category,CancellationToken cancellation);
    Task<List<Category>> GetUserCategoriesAsync(Guid userId,CancellationToken cancellation);
    Task<Category?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    void DeleteAsync(Category category);
}