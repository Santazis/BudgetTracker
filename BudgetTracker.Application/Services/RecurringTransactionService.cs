using System.Diagnostics;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.DomainServices;
using BudgetTracker.Domain.Models.RecurringTransaction;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services;

public class RecurringTransactionService : IRecurringTransactionService
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecurringCronScheduleCalculator _cronScheduleCalculator;
    private readonly ITransactionRepository _transactionRepository;

    public RecurringTransactionService(IRecurringTransactionRepository recurringTransactionRepository,
        IUnitOfWork unitOfWork, IRecurringCronScheduleCalculator cronScheduleCalculator,
        ITransactionRepository transactionRepository)
    {
        _recurringTransactionRepository = recurringTransactionRepository;
        _unitOfWork = unitOfWork;
        _cronScheduleCalculator = cronScheduleCalculator;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<RecurringTransactionDto>> GetByUserIdAsync
    (Guid userId, PaginationRequest request,
        CancellationToken cancellation)
    {
        var transactions = await _recurringTransactionRepository.GetByUserIdAsync(userId, request, cancellation);
        return transactions.Select(RecurringTransactionDto.FromEntity);
    }

    public async Task CreateRecurringTransactionAsync(CreateRecurringTransaction request, Guid userId,
        CancellationToken cancellation)
    {
        var runDate = _cronScheduleCalculator.CalculateRunDate(request.CronExpression);
        var createdTransactions = new List<RecurringTransaction>();
        var bufferSize = 1000;

        for (int i = 0; i < 30000; i++)
        {
            var transaction = RecurringTransaction.Create(
                userId: userId,
                categoryId: request.CategoryId,
                description: request.Description,
                amount: Money.Create(request.Amount, request.Currency),
                paymentMethodId: request.PaymentMethodId,
                cronExpression: request.CronExpression,
                nextRun: runDate
            );
            createdTransactions.Add(transaction);
            if (createdTransactions.Count == bufferSize)
            {
                await _recurringTransactionRepository.AddRangeAsync(createdTransactions, cancellation);
                await _unitOfWork.SaveChangesAsync(cancellation);
                createdTransactions.Clear();
            }
        }
    }

    public async Task CreateTransactionFromRecurringTransactionAsync(Guid recurringTransactionId, Guid userId,
        CancellationToken cancellation)
    {
        var recurringTransaction =
            await _recurringTransactionRepository.GetByIdAsync(recurringTransactionId, userId, cancellation);

        if (recurringTransaction == null)
        {
            throw new RequestException("Recurring Transaction Not Found");
        }


        var transaction = Transaction.Create(
            amount: recurringTransaction.Amount,
            createdAt: DateTime.UtcNow,
            paymentMethodId: recurringTransaction.PaymentMethodId,
            categoryId: recurringTransaction.CategoryId,
            userId: userId,
            description: recurringTransaction.Description
        );

        var nextRunDate = _cronScheduleCalculator.CalculateRunDate(recurringTransaction.CronExpression);
        recurringTransaction.UpdateLastRun();
        recurringTransaction.UpdateNextRun(nextRunDate);
        await _transactionRepository.CreateAsync(transaction, cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public Task ProcessRecurringTransactionsWithBatchesAsync(CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }


    public async Task DeleteAsync(Guid recurringTransactionId, Guid userId, CancellationToken cancellation)
    {
        var transaction =
            await _recurringTransactionRepository.GetByIdAsync(recurringTransactionId, userId, cancellation);
        if (transaction is null)
        {
            throw new RequestException("Recurring Transaction Not Found");
        }

        _recurringTransactionRepository.Delete(transaction);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public async Task UpdateAsync(Guid recurringTransactionId, Guid userId, UpdateRecurringTransaction request,
        CancellationToken cancellation)
    {
        var transaction =
            await _recurringTransactionRepository.GetByIdAsync(recurringTransactionId, userId, cancellation);
        if (transaction is null)
        {
            throw new RequestException("Recurring Transaction Not Found");
        }

        var amount = Money.Create(request.Amount, request.Currency);
        transaction.Update(request.Description, amount, request.CronExpression);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }
    
}