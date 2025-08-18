using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Application.Models.Category.Requests;

public class UpdateCategory
{
    public string Name { get; set; } = string.Empty;
    public CategoryTypes Type { get; set; }
}