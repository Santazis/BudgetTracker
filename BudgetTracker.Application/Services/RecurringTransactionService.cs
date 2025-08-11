using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;
using BudgetTracker.Domain.Common.Exceptions;
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
        IUnitOfWork unitOfWork, IRecurringCronScheduleCalculator cronScheduleCalculator, ITransactionRepository transactionRepository)
    {
        _recurringTransactionRepository = recurringTransactionRepository;
        _unitOfWork = unitOfWork;
        _cronScheduleCalculator = cronScheduleCalculator;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<RecurringTransactionDto>> GetRecurringTransactionsAsync(Guid userId,
        CancellationToken cancellation)
    {
        var transactions = await _recurringTransactionRepository.GetByUserIdAsync(userId, cancellation);
        return transactions.Select(RecurringTransactionDto.FromEntity);
    }

    public async Task CreateRecurringTransactionAsync(CreateRecurringTransaction request, Guid userId,
        CancellationToken cancellation)
    {
        var runDate = _cronScheduleCalculator.CalculateRunDate(request.CronExpression);
        Console.WriteLine(runDate);
        var transaction = RecurringTransaction.Create(
            userId: userId,
            categoryId: request.CategoryId,
            description: request.Description,
            amount: Money.Create(request.Amount, request.Currency),
            paymentMethodId: request.PaymentMethodId,
            cronExpression: request.CronExpression,
            nextRun: runDate
        );
        
        await _recurringTransactionRepository.CreateAsync(transaction,cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
    }

    public async Task CreateTransactionFromRecurringTransactionAsync(Guid recurringTransactionId, Guid userId,
        CancellationToken cancellation)
    {
        var recurringTransaction = await _recurringTransactionRepository.GetByIdAsync(recurringTransactionId,userId, cancellation);
    
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
}