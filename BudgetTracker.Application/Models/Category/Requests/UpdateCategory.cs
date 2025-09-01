using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Application.Models.Category.Requests;

public record UpdateCategory
(string Name,CategoryTypes Type);