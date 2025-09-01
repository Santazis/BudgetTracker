using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Models.SavingGoal;

public sealed class SavingGoal : Entity
{
    private SavingGoal(Guid id):base(id){}
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Money TargetAmount { get; private set; }
    public BudgetPeriod Period { get; private set; }
    public bool IsActive { get; private set; }
    public Guid UserId { get; private set; }
    public User.User User { get; private set; }
    
    private List<SavingGoalTransaction> _transactions { get; set; } = new();
    public IReadOnlyCollection<SavingGoalTransaction> Transactions => _transactions;

    public static SavingGoal Create(Guid userId, string name, Money targetAmount, BudgetPeriod period,
        string? description)
    {
        return new SavingGoal(Guid.NewGuid())
        {
            Name = name,
            UserId = userId,
            Description = description,
            Period = period,
            _transactions = new(),
            TargetAmount = targetAmount,
            IsActive = true,
        };
    }

    public void AddTransaction(SavingGoalTransaction transaction)
    {
        _transactions.Add(transaction);
    }
    
    public void DeleteTransaction(Guid transactionId)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);
        if (transaction is null) throw new ArgumentException("Transaction not found");
        _transactions.Remove(transaction);
    }
    
    public Money CurrentAmount => _transactions.Aggregate(Money.Create(0, TargetAmount.Currency),
        (acc, t) => acc + t.Amount);

    public Money AmountLeft => TargetAmount - CurrentAmount;
}