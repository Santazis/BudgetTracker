namespace BudgetTracker.Domain.Common.Errors;

public static class BudgetErrors
{
    public static readonly Error BudgetNotFound = new("Budget.NotFound","Budget not found");
}