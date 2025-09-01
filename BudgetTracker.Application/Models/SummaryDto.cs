using BudgetTracker.Application.Models.Category;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models;

public record SummaryDto(
    Money Total,
    Money TotalIncome,
    Money TotalExpense,
    IEnumerable<CategorySummary> ByCategory);