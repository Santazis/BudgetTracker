namespace BudgetTracker.Domain.Common.Errors;

public static class CategoryErrors
{
    public static readonly Error CategoryNotFound = new("Category.NotFound","Category not found");
    public static readonly Error TryingUpdateSystemCategory = new ("Category.TryingUpdateSystemCategory", "Cant update system category");
    
}