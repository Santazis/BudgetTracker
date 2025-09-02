namespace BudgetTracker.Domain.Common.Errors;

public class SavingGoalErrors
{
    public static readonly Error SavingGoalNotFound = new("SavingGoal.NotFound","Saving goal not found");
}