namespace BudgetTracker.Domain.Common.Errors;

public static class TransactionErrors
{
    public static readonly Error TransactionNotFound = new("Transaction.NotFound","Transaction not found");
}