using System.Globalization;
using BudgetTracker.Application.Models.Transaction;
using CsvHelper.Configuration;

namespace BudgetTracker.Infrastructure;

public sealed class TransactionCsvImportMap : ClassMap<TransactionCsvImport>
{
    public TransactionCsvImportMap(int nameIndex, int dateIndex, int amountIndex, int descriptionIndex)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(nameIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(dateIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(amountIndex);
        ArgumentOutOfRangeException.ThrowIfNegative(descriptionIndex);

        Map(m => m.Amount).Index(amountIndex);
        Map(m => m.Date).TypeConverterOption
            .DateTimeStyles(DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal).Index(dateIndex);
        Map(m => m.Name).Index(nameIndex);
        Map(m => m.Description).Optional().Index(descriptionIndex);
    }
}