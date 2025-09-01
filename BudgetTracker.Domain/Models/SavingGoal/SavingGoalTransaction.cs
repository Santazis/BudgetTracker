using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Models.SavingGoal;

public class SavingGoalTransaction : Entity
{
    private SavingGoalTransaction(Guid id) : base(id) {}
    public Money Amount { get; private set; }
    public Guid SavingGoalId { get; private set; }
    public SavingGoal SavingGoal { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public string? Description { get; private set; }

    public static SavingGoalTransaction Create(Guid savingGoalId, Money amount, string? description)
    {
        return new SavingGoalTransaction(Guid.NewGuid())
        {
            Amount = amount,
            SavingGoalId = savingGoalId,
            Description = description,
            CreatedAt = DateTime.UtcNow,
        };
    }
}