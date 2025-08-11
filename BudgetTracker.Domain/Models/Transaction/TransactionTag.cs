namespace BudgetTracker.Domain.Models.Transaction;

public class TransactionTag
{
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; }
    
    public Guid TagId { get; set; }
    public Tag Tag { get; set; }
    
    public TransactionTag(Guid transactionId, Guid tagId)
    {
        TransactionId = transactionId;
        TagId = tagId;
    }
}