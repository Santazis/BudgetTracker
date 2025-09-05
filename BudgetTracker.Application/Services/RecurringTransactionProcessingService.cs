using System.Diagnostics;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Domain.DomainServices;
using BudgetTracker.Domain.Models.RecurringTransaction;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace BudgetTracker.Application.Services;

public class RecurringTransactionProcessingService : IRecurringTransactionProcessingService
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRecurringCronScheduleCalculator _cronScheduleCalculator;
    private readonly ILogger<RecurringTransactionProcessingService> _logger;

    public RecurringTransactionProcessingService(
        ITransactionRepository transactionRepository, 
        IRecurringTransactionRepository recurringTransactionRepository, 
        IUnitOfWork unitOfWork, 
        IRecurringCronScheduleCalculator cronScheduleCalculator,
        ILogger<RecurringTransactionProcessingService> logger)
    {
        _transactionRepository = transactionRepository;
        _recurringTransactionRepository = recurringTransactionRepository;
        _unitOfWork = unitOfWork;
        _cronScheduleCalculator = cronScheduleCalculator;
        _logger = logger;
    }


public class ProcessingResult
{
    public int ProcessedCount { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}

public class BatchSaveResult
{
    public bool IsSuccess { get; set; }
    public int SavedCount { get; set; }
    public string ErrorMessage { get; set; }
}

    public async Task<ProcessingResult> ProcessRecurringTransactionsByCursorPaginationAsync(CancellationToken cancellation)
    {
        var watch = new Stopwatch();
        watch.Start();
        var processedTransactions = 0;
        var batchSize = 500;
        Guid? lastId = null;
        DateTime? lastRun = null;
        var now = DateTime.UtcNow.Date;
        var end = DateTime.UtcNow.Date.AddDays(1);

        _logger.LogInformation("Starting recurring transaction processing for date {ProcessingDate}", now);

        do
        {
            cancellation.ThrowIfCancellationRequested();
            var transactions =
                await _recurringTransactionRepository.GetByCursorAsync(lastId, lastRun, batchSize, now, end,
                    cancellation);
            if (transactions.Count == 0)
            {
                _logger.LogInformation("No more transactions to process");
                break;
            }

            _logger.LogDebug("Processing batch with {TransactionCount} transactions", transactions.Count);

            var createdTransactions = new List<Transaction>();
            var processedRecurring = new List<RecurringTransaction>();
            foreach (var recurringTransaction in transactions)
            {
                processedTransactions++;
                var transaction = Transaction.Create(
                    amount: recurringTransaction.Amount,
                    createdAt: DateTime.UtcNow.AddMonths(-1),
                    paymentMethodId: recurringTransaction.PaymentMethodId,
                    categoryId: recurringTransaction.CategoryId,
                    userId: recurringTransaction.UserId,
                    description: recurringTransaction.Description);

                foreach (var tag in recurringTransaction.RecurringTransactionTags)
                {
                    transaction.AttachTag(tag.TagId);
                }
                var nextDate = _cronScheduleCalculator.CalculateRunDate(recurringTransaction.CronExpression);
                recurringTransaction.UpdateLastRun();
                recurringTransaction.UpdateNextRun(nextDate);
                createdTransactions.Add(transaction);
                processedRecurring.Add(recurringTransaction);
            }

            var batchResult = await SaveByBatchesAsync(createdTransactions, processedRecurring, cancellation);
            lastId = transactions.Last().Id;
            lastRun = transactions.Last().NextRun;
            await Task.Delay(200, cancellation);
        } while (true);

        watch.Stop();
        var result = new ProcessingResult
        {
            ProcessedCount = processedTransactions,
            Duration = watch.Elapsed,
            IsSuccess = true
        };
        
        _logger.LogInformation("Recurring transaction processing completed. Processed: {ProcessedCount}, Duration: {Duration}ms", 
            result.ProcessedCount, result.Duration.TotalMilliseconds);
        return result;
    }

    private async Task<BatchSaveResult> SaveByBatchesAsync(List<Transaction> transactions,
        List<RecurringTransaction> recurringTransactions, CancellationToken cancellation)
    {
        await using var dbTransaction = await _unitOfWork.BeginTransactionAsync(cancellation);

        try
        {
            await _transactionRepository.CreateRangeAsync(transactions, cancellation);
            _recurringTransactionRepository.UpdateRange(recurringTransactions);
            await _unitOfWork.SaveChangesAsync(cancellation);
            await dbTransaction.CommitAsync(cancellation);
            
            _logger.LogDebug("Successfully saved batch with {TransactionCount} transactions", transactions.Count);
            
            return new BatchSaveResult
            {
                IsSuccess = true,
                SavedCount = transactions.Count
            };
        }
        catch (Exception e)
        {
            await dbTransaction.RollbackAsync(cancellation);
            _logger.LogError(e, "Failed to save batch with {TransactionCount} transactions", transactions.Count);
            Console.WriteLine(e);
            Console.WriteLine("Error count" + transactions.Count);
            
            return new BatchSaveResult
            {
                IsSuccess = false,
                SavedCount = 0,
                ErrorMessage = e.Message
            };
        }
    }
}