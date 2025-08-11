namespace BudgetTracker.Application.Models.Transaction;

public record TransactionImportResult( 
IEnumerable<ParsedTransactionResult> ParsedTransactions,
IEnumerable<string> Errors,
int UploadedCount,
int ErrorCount );