using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Application.Services.CategoryServices;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest.CategoryTests;

public class UpdateAsyncTest
{
    private static readonly UpdateCategory request = new("Name", CategoryTypes.Expense);
    private static readonly Category category = Category.Create(Guid.NewGuid(), "name", CategoryTypes.Expense);
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ICategoryRepository _categoryRepositoryMock;

    public UpdateAsyncTest()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnError_WhenCategoryNotFound()
    {
        //arrange
        var userId = Guid.NewGuid();

        _categoryRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == category.Id), Arg.Is<Guid>(u => u == userId),
                Arg.Any<CancellationToken>())
            .Returns((Category?)null);
        
        //act
        var result = await _categoryService.UpdateCategoryAsync(request,category.Id,userId, CancellationToken.None);
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }
    [Fact]
    public async Task UpdateAsync_Should_CallUpdateAndSaveChanges()
    {
        //arrange
        var userId = Guid.NewGuid();
        _categoryRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == category.Id), Arg.Is<Guid>(u => u == userId),
                Arg.Any<CancellationToken>())
            .Returns(category);
        //act
        var result = await _categoryService.UpdateCategoryAsync(request,category.Id,userId, CancellationToken.None);
        //assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
}