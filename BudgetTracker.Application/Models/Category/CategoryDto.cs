using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Application.Models.Category;

public record CategoryDto(string Name,Guid Id, string Type);
