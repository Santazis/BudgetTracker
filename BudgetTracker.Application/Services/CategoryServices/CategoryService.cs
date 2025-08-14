using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<CategoryDto> CreateAsync(CreateCategory request, CancellationToken cancellation)
    {
        var category = Category.Create(
            isSystem:false,
            name:request.Name,
            userId:request.UserId,
            type:request.Type);
        await _categoryRepository.CreateAsync(category,cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return new CategoryDto(category.Name,category.Id,category.Type.ToString());
    }

    public async Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellation)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId,cancellation);
        return categories.Select(c=> new CategoryDto(c.Name,c.Id,c.Type.ToString()));
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(id,userId,cancellation);
        if (category is null) throw new RequestException("Category not found");
        return new CategoryDto(category.Name,category.Id,category.Type.ToString());
    }

    public async Task DeleteAsync(Guid categoryId,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId,userId,cancellation);
        if(category is null) throw new RequestException("Category not found");
        _categoryRepository.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(string name,Guid categoryId,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId,userId,cancellation);
        if (category is null) throw new RequestException("Category not found");
        if (category.IsSystem) throw new RequestException("Cannot update system category");
        category.Update(name);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return new CategoryDto(category.Name,category.Id,category.Type.ToString());
    }

    public Task DeleteAsync(Guid categoryId, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}