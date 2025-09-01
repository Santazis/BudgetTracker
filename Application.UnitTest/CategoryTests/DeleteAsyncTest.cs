using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Services.CategoryServices;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Repositories;
using DomainCategory =   BudgetTracker.Domain.Models.Category;

using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest.CategoryTests;

public class DeleteAsyncTest
{
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ICategoryService _categoryService;
    private readonly ICategoryRepository _categoryRepositoryMock;

    public DeleteAsyncTest()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnError_WhenCategoryNotFound()
    {
        //arrange
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _categoryRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == categoryId), Arg.Is<Guid>(u=> u == userId),Arg.Any<CancellationToken>())
            .Returns((DomainCategory.Category?)null);
        //act
        var result = await _categoryService.DeleteAsync(categoryId, userId, CancellationToken.None);
        
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }

    [Fact]
    public async Task DeleteAsync_Should_CallDeleteAndSaveChanges()
    {
        //arrange
        var userId = Guid.NewGuid();
        var category = DomainCategory.Category.Create(userId, "name", CategoryTypes.Expense);
        
        _categoryRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == category.Id), Arg.Is<Guid>(u=> u == userId),Arg.Any<CancellationToken>())
            .Returns(category);
        //act
        var result = await _categoryService.DeleteAsync(category.Id,userId, CancellationToken.None);
        //assert
        _categoryRepositoryMock.Received(1).DeleteAsync(Arg.Is<DomainCategory.Category>(category));
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
}