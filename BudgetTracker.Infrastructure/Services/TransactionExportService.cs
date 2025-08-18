using System.Globalization;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;
using CsvHelper;
using CsvHelper.Configuration;

namespace BudgetTracker.Infrastructure.Services;

public class TransactionExportService : ITransactionExportService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionExportService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task ExportToCsvAsync(Guid userId, TransactionFilter filter,Stream output, CancellationToken cancellation)
    {
        var transactions =  _transactionRepository.GetTransactionsAsync( filter,userId, cancellation);
        await using var streamWriter = new StreamWriter(output, leaveOpen: true);
        await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);
        csvWriter.Context.RegisterClassMap<TransactionExportMap>();
        csvWriter.WriteHeader<Transaction>();
        await csvWriter.NextRecordAsync();
        await foreach (var transaction in transactions)
        {
            cancellation.ThrowIfCancellationRequested();
            csvWriter.WriteRecord(transaction);
            await csvWriter.NextRecordAsync();
        }
        await streamWriter.FlushAsync(cancellation);
    }
}