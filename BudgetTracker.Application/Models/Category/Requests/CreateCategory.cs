using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Application.Models.Category.Requests;

public record CreateCategory(string Name,CategoryTypes Type);