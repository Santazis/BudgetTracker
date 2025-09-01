using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
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


    public async Task<Result<CategoryDto>> CreateAsync(CreateCategory request,Guid userId, CancellationToken cancellation)
    {
        var category = Category.Create(
            name:request.Name,
            userId:userId,
            type:request.Type);
        await _categoryRepository.CreateAsync(category,cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result<CategoryDto>.Success(new CategoryDto(category.Name,category.Id,category.Type.ToString()));
    }

    public async Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellation)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId,cancellation);
        return categories.Select(c=> new CategoryDto(c.Name,c.Id,c.Type.ToString()));
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(id,userId,cancellation);
        if (category is null) return Result<CategoryDto>.Failure(CategoryErrors.CategoryNotFound);
        return Result<CategoryDto>.Success(new CategoryDto(category.Name,category.Id,category.Type.ToString()));
    }

    public async Task<Result> DeleteAsync(Guid categoryId,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId,userId,cancellation);
        if (category is null) return Result<CategoryDto>.Failure(CategoryErrors.CategoryNotFound);
        _categoryRepository.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<Result<CategoryDto>> UpdateCategoryAsync(UpdateCategory request,Guid categoryId,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId,userId,cancellation);
        if (category is null) return Result<CategoryDto>.Failure(CategoryErrors.CategoryNotFound);
        category.Update(request.Name,request.Type);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result<CategoryDto>.Success(new CategoryDto(category.Name,category.Id,category.Type.ToString()));
    }
    
}