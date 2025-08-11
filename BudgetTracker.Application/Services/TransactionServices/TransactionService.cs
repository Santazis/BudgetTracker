using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Services.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter? filter, CancellationToken cancellation)
    {
        var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId,filter,cancellation);
        
        return transactions.Select(TransactionDto.FromEntity);
    }

    public async Task<TransactionDto> CreateTransactionAsync(CreateTransaction request,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId,userId,cancellation);
        if (category is null)
        {
            throw new RequestException("Category not found");
        }
        var transaction = Transaction.Create(
            amount: Money.Create(request.Amount, request.Currency),
            categoryId: request.CategoryId,
            userId: userId,
            createdAt:DateTime.UtcNow, 
            paymentMethodId:request.PaymentMethodId,
            description: request.Description);
        await _transactionRepository.CreateAsync(transaction, cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return  TransactionDto.FromEntity(transaction);
    }

    public async Task DeleteTransactionAsync(Guid transactionId,Guid userId, CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId,cancellation);
        if (transaction is null)
        {
            throw new RequestException("Transaction not found");
        }
        _transactionRepository.Delete(transaction);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public async Task<TransactionDto> UpdateTransactionAsync(Guid transactionId,Guid userId, UpdateTransaction request,
        CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId,cancellation);
        if (transaction is null)
        {
            throw new RequestException("Transaction not found");
        }
        
        
        var amountMoney = Money.Create(request.Amount, request.Currency);
        transaction.Update(
            amount:amountMoney,
            description:request.Description);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return TransactionDto.FromEntity(transaction);
    }

    public async Task AttachPaymentMethodAsync(Guid transactionId, Guid userId, Guid paymentMethodId, CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId,cancellation);
        if (transaction is null)
        {
            throw new RequestException("Transaction not found");
        }
        transaction.AttachPaymentMethod(paymentMethodId);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public async Task AttachTagAsync(Guid transactionId, Guid userId, Guid tagId, CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId,cancellation);
        if (transaction is null)
        {   
            throw new RequestException("Transaction not found");
        }
        transaction.AttachTag(tagId);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public async Task UploadMassTransactionsAsync(Guid userId, IEnumerable<CreateTransaction> request, CancellationToken cancellation)
    {
        var batchSize = 100;
        foreach (var transactionRequest in request.Chunk(batchSize))
        {
         var transactions =   transactionRequest.Select(t=> Transaction.Create(
            amount:Money.Create(t.Amount,"USD"),
            createdAt:t.CreatedAt,
            categoryId:t.CategoryId,
            paymentMethodId:t.PaymentMethodId,
            description:t.Description,
            userId:userId));
         await _transactionRepository.CreateRangeAsync(transactions,cancellation);
         await _unitOfWork.SaveChangesAsync(cancellation);
        }
        // var transactions = request.ToList();
        // var tasks = new List<Task>();
        // for (var i = 0; i < transactions.Count; i+=batchSize)
        // {
        //     var batch = transactions.Skip(i).Take(batchSize).Select(t=> Transaction.Create(
        //         amount:Money.Create(t.Amount,"USd"),
        //         createdAt:t.CreatedAt,
        //         categoryId:t.CategoryId,
        //         paymentMethodId:t.PaymentMethodId,
        //         description:t.Description,
        //         userId:userId));
        //     tasks.Add(_transactionRepository.CreateRangeAsync(batch,cancellation));
        // }
        // await Task.WhenAll(tasks);
        // await _unitOfWork.SaveChangesAsync(cancellation);
    }
}