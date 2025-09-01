using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Application.Services.TransactionServices;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using NSubstitute;

namespace Application.UnitTest.TransactionTests;

public class UploadMassTransactionsAsyncTest
{
    private readonly ITransactionService _transactionService;
    private readonly ITransactionRepository _transactionRepositoryMock;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public UploadMassTransactionsAsyncTest()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _transactionRepositoryMock = Substitute.For<ITransactionRepository>();
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _transactionService = new TransactionService(_transactionRepositoryMock, _unitOfWorkMock,_categoryRepositoryMock);
    }

    [Fact]
    public async Task UploadMassTransactionsAsync_Should_CallCreateAndSaveChanges_PerBatch()
    {
        //arrange
        var userId = Guid.NewGuid();
        var requests = Enumerable.Range(1, 250).Select(t =>
            new CreateTransaction(null, DateTime.UtcNow, 250, "usd", $"desc {t}", Guid.NewGuid()));
        // var transactions = requests.Select(t=> Transaction.Create(t.Amount,t.CreatedAt,t.PaymentMethodId,t.CategoryId,userId,t.Description));
        //act
        await _transactionService.UploadMassTransactionsAsync(userId,requests, CancellationToken.None);
        //arrange
        await _transactionRepositoryMock.Received(3).CreateRangeAsync(Arg.Any<IEnumerable<Transaction>>(), Arg.Any<CancellationToken>());
        await _unitOfWorkMock.Received(3).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}