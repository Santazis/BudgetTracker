namespace BudgetTracker.Domain.Common.Errors;

public static class RecurringTransactionErrors
{
    public static readonly Error RecurringTransactionNotFound = new("RecurringTransaction.NotFound","Recurring transaction not found");
}