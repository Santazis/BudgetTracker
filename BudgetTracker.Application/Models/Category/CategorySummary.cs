using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.Category;

public record CategorySummary(
    CategoryDto Category,
    Money TotalAmount
);