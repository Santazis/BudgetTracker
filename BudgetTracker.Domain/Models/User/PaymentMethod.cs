using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Domain.Models.User;

public class PaymentMethod {
    
    internal PaymentMethod(Guid id,Guid userId, PaymentMethodType type, string name, string? details)

    {
        Id = id;
        UserId = userId;
        Type = type;
        Name = name;
        Details = details;
    }
    public Guid Id { get; set; } 
    public Guid UserId { get;set; }
    public User User { get;set; } = null!;
    public PaymentMethodType Type { get;set; }
    public string Name { get; set; }
    public string? Details { get; set; }

}