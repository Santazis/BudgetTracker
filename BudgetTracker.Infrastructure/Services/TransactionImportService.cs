using System.Globalization;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using CsvHelper;
using CsvHelper.Configuration;

namespace BudgetTracker.Infrastructure.Services;

public class TransactionImportService : ITransactionImportService
{
    public async Task<TransactionImportResult> ImportFromCsvAsync(Stream csvStream, Guid userId, CancellationToken cancellation)
    {
        var failedLines = new List<string>();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim, // Trims whitespace
            MissingFieldFound = null, // Don't throw on missing fields
            HeaderValidated = null, // Don't validate headers
            BadDataFound = null,
            Delimiter = ",",
            HasHeaderRecord = false,
            ReadingExceptionOccurred = args => {failedLines.Add(args.Exception.Context.Parser.RawRecord);
                return false;
            }
        };
        using var streamReader = new StreamReader(csvStream);
        using var csvReader = new CsvReader(streamReader, config);
        var csvMap = new TransactionCsvImportMap(0,3,2,1);
        csvReader.Context.RegisterClassMap(csvMap);
        var record =  csvReader.GetRecordsAsync<TransactionCsvImport>(cancellation);
        var parsedTransactions = new List<ParsedTransactionResult>();
        await foreach (var rec in record)
        {
            
            var transaction = new ParsedTransactionResult(
                Name:rec.Name,
                Description:rec.Description,
                Amount:rec.Amount,
                Date:rec.Date,
                CategoryId:null,
                PaymentMethodId:null);
            parsedTransactions.Add(transaction);
        }
        return new TransactionImportResult(parsedTransactions,failedLines,parsedTransactions.Count,failedLines.Count);
    }
}