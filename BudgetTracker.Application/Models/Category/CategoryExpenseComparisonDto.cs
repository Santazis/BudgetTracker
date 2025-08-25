namespace BudgetTracker.Application.Models.Category;

public record CategoryExpenseComparisonDto(CategoryDto Category, decimal CurrentMonth, decimal PreviousMonth)
{
 public decimal Difference => PreviousMonth - CurrentMonth;
 public double Percentage => PreviousMonth == 0 ?  0: (double)((PreviousMonth - CurrentMonth) / PreviousMonth * 100);
}