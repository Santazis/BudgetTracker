using BudgetTracker.Domain.Models;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Repositories.Models;

public record CategoryMonthlyComparison
(
    Category Category,
    decimal PreviousMonth,
    decimal CurrentMonth

);