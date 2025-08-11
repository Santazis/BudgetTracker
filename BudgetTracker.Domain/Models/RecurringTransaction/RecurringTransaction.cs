using BudgetTracker.Domain.DomainServices;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Domain.Models.RecurringTransaction;

public class RecurringTransaction : Entity
{
    private RecurringTransaction(Guid id) : base(id) {}
    
    public Guid UserId { get; private set; }
    public User.User User { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Category.Category Category { get; private set; } = null!;
    public string? Description { get; private set; }
    public Money Amount { get; private set; } = null!;
    public string CronExpression { get; private set; } = string.Empty;
    public PaymentMethod? PaymentMethod { get; private set; }
    public Guid? PaymentMethodId { get; private set; }
    private List<RecurringTransactionTag> _recurringTransactionTags = new();
    public IReadOnlyCollection<RecurringTransactionTag> RecurringTransactionTags => _recurringTransactionTags;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastRun { get; private set; }
    public DateTime? NextRun { get; private set; }
    
    public static RecurringTransaction Create(Guid userId, Guid categoryId, 
        string? description, Money amount, 
        string cronExpression, 
        DateTime nextRun,
        Guid? paymentMethodId
        )
    {
        return new RecurringTransaction(Guid.NewGuid())
        {
            UserId = userId,
            CategoryId = categoryId,
            Description = description,
            Amount = amount,
            CronExpression = cronExpression,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            NextRun = nextRun,
            PaymentMethodId = paymentMethodId,
        };
    }
    public void UpdateLastRun()
    {
        LastRun = DateTime.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
        NextRun = null;
    }

    public  void Activate(DateTime nextRun)
    {
        IsActive = true;
        NextRun = nextRun;
    }

    public void UpdateNextRun(DateTime nextRun)
    {
        NextRun = nextRun;
    }
    public void Update(string? description, Money amount, string cronExpression)
    {
        if(Description == description && Amount == amount && CronExpression == cronExpression) return;
        Description = description;
        Amount = amount;
        CronExpression = cronExpression;
    }
}