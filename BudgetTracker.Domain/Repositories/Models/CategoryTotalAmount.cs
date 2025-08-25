using BudgetTracker.Domain.Models.Category;

namespace BudgetTracker.Domain.Repositories.Models;

public record CategoryTotalAmount
(Category Category, decimal Amount);