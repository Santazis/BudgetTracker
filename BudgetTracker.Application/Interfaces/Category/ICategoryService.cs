using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;

namespace BudgetTracker.Application.Interfaces.Category;
using Domain.Models.Category;

public interface ICategoryService
{
    Task<CategoryDto> CreateAsync(CreateCategory request,Guid userId,CancellationToken cancellation);
    Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId,CancellationToken cancellation);
    Task<CategoryDto> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    Task DeleteAsync(Guid categoryId,CancellationToken cancellation);
    Task<CategoryDto> UpdateCategoryAsync(UpdateCategory request,Guid categoryId,Guid userId, CancellationToken cancellation);
}