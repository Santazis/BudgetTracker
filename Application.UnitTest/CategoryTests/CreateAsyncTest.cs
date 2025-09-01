using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Category;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Category.Requests;
using BudgetTracker.Application.Services.CategoryServices;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest.CategoryTests;

public class CreateAsyncTest
{
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ICategoryRepository _categoryRepositoryMock;

    public CreateAsyncTest()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task CreateAsync_Should_CallCreateAndSaveChanges()
    {
        //arrange
        var userId = Guid.NewGuid();
        var request = new CreateCategory("Name", CategoryTypes.Expense);
        //act 
        var result = await _categoryService.CreateAsync(request, userId, CancellationToken.None);
        //assert
        await _categoryRepositoryMock.Received(1).CreateAsync(Arg.Is<Category>(c => c.Name == request.Name && c.UserId == userId && c.Type == request.Type), Arg.Any<CancellationToken>());
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
    }
}