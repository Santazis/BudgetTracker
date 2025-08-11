namespace BudgetTracker.Application.Models.Filters;

public class SummaryFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public IEnumerable<Guid>? Categories { get; set; }
}