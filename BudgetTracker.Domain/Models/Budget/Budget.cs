namespace BudgetTracker.Domain.Models.Budget;
using User;
using Category; 
using Transaction;
public class Budget : Entity
{
    private Budget(Guid id) : base(id) {}
    
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money LimitAmount { get; private set; } = null!;
    public BudgetPeriod Period { get; private set; }  = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static Budget Create(Guid userId,string name,string? description, Guid categoryId, Money limitAmount, BudgetPeriod period)
    {

        if(limitAmount.Amount < 0) throw new ArgumentException("Limit amount must be greater than 0");
        return new Budget(Guid.NewGuid())
        {
            UserId = userId,
            CategoryId = categoryId,
            LimitAmount = limitAmount,
            Name = name,
            Description = description,
            Period = period,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public void Update(Money limitAmount,string name,string? description, BudgetPeriod period)
    {
        if(LimitAmount == limitAmount && Period == period) return;
        LimitAmount = limitAmount;
        Period = period;
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
    }
    
    public bool IsLimitExceeded(Money spent)
    {
        return spent > LimitAmount;
    }
    
    public bool IsOwner(Guid userId)
    {
        return userId == UserId;
    }
}