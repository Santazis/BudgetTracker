using BudgetTracker.Application.Interfaces;
using Hangfire;
using Microsoft.Extensions.Hosting;

namespace BudgetTracker.Infrastructure.BackgroundServices;
[AutomaticRetry(Attempts = 5)]
public class RecurringTransactionsBackgroundService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // RecurringJob.AddOrUpdate<IRecurringTransactionProcessingService>(
        //     "RecurringTransactionProcessingService",
        //     service => service.ProcessRecurringTransactionsByCursorPaginationAsync(cancellationToken),
        //     Cron.Daily());
        //
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}