using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Models.User;

public record PaymentMethodDto(Guid Id, string Name, string? Details, string Type)
{
    public static PaymentMethodDto FromEntity(PaymentMethod p)=>
        new(p.Id,p.Name,p.Details,p.Type.ToString());
}