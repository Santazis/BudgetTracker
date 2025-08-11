using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Models.RecurringTransaction;

public class RecurringTransactionTag
{
    public RecurringTransaction RecurringTransaction { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
    
    public Guid RecurringTransactionId { get; set; }
    public Guid TagId { get; set; }
    
    public RecurringTransactionTag(Guid recurringTransactionId, Guid tagId)
    {
        RecurringTransactionId = recurringTransactionId;
        TagId = tagId;
    }
}