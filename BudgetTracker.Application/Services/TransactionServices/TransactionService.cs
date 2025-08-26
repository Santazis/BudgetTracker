using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Common.Pagination;
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

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter? filter,PaginationRequest request, CancellationToken cancellation)
    {
        var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId,filter,request,cancellation);
        
        return transactions.Select(TransactionDto.FromEntity);
    }

    public async Task<Result<TransactionDto>> CreateTransactionAsync(CreateTransaction request,Guid userId, CancellationToken cancellation)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId,userId,cancellation);
        if (category is null)
        {
            return Result<TransactionDto>.Failure(CategoryErrors.CategoryNotFound);
        }
        var transaction = Transaction.Create(
            amount: Money.Create(request.Amount, request.Currency),
            categoryId: request.CategoryId,
            userId: userId,
            createdAt:request.CreatedAt,
            paymentMethodId:request.PaymentMethodId,
            description: request.Description);
        await _transactionRepository.CreateAsync(transaction, cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return  Result<TransactionDto>.Success(TransactionDto.FromEntity(transaction));
    }

    public async Task<Result> DeleteTransactionAsync(Guid transactionId,Guid userId, CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId,cancellation);
        if (transaction is null)
        {
            return Result.Failure(TransactionErrors.TransactionNotFound);
        }
        _transactionRepository.Delete(transaction);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<Result<TransactionDto>> UpdateTransactionAsync(Guid transactionId,Guid userId, UpdateTransaction request,
        CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId,cancellation);
        if (transaction is null)
        {
            return Result<TransactionDto>.Failure(TransactionErrors.TransactionNotFound);
        }
        
        
        var amountMoney = Money.Create(request.Amount, request.Currency);
        transaction.Update(
            amount:amountMoney,
            description:request.Description);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result<TransactionDto>.Success(TransactionDto.FromEntity(transaction)); 
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
    }
    
}