namespace BudgetTracker.Domain.Repositories.Filters;

public class TransactionFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public HashSet<Guid>? Categories { get; set; }
    public HashSet<Guid>? Tags { get; set; }
    public HashSet<Guid>? PaymentMethods { get; set; }
}