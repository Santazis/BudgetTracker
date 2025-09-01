using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces.Category;
using Domain.Models.Category;

public interface ICategoryService
{
    Task<Result<CategoryDto>> CreateAsync(CreateCategory request,Guid userId,CancellationToken cancellation);
    Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId,CancellationToken cancellation);
    Task<Result<CategoryDto>> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    Task<Result> DeleteAsync(Guid categoryId,Guid userId,CancellationToken cancellation);
    Task<Result<CategoryDto>> UpdateCategoryAsync(UpdateCategory request,Guid categoryId,Guid userId, CancellationToken cancellation);
}