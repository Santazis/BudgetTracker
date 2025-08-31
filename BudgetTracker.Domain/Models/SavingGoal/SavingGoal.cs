using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Models.SavingGoal;

public sealed class SavingGoal : Entity
{
    private SavingGoal(Guid id):base(id){}
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Money CurrentAmount { get; private set; }
    public Money TargetAmount { get; private set; }
    public BudgetPeriod Period { get; private set; }
    public bool IsActive { get; private set; }
    public Guid UserId { get; private set; }
    public User.User User { get; private set; }
}