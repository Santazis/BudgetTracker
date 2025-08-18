namespace BudgetTracker.Domain.Repositories.Filters;

public class TransactionFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public HashSet<Guid>? Categories { get; set; }
    public HashSet<Guid>? Tags { get; set; }
    public HashSet<Guid>? PaymentMethods { get; set; }
    public string? SortBy { get; set; }
    public bool? IsIncome { get; set; }
    public bool Descending { get; set; } = true;
}