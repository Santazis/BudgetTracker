namespace BudgetTracker.Domain.Models.Budget;

public class BudgetPeriod : ValueObject
{
    private BudgetPeriod(DateTime periodStart, DateTime periodEnd)
    {
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }
    public DateTime PeriodStart { get; private set; }
    public DateTime PeriodEnd { get; private set; }

    public static BudgetPeriod Create(DateTime periodStart, DateTime periodEnd)
    {
        if(periodStart > periodEnd) throw new ArgumentException("Period start must be before period end");
        if(periodStart < DateTime.UtcNow) throw new ArgumentException("Period start must be in the future");
        if(periodEnd < DateTime.UtcNow) throw new ArgumentException("Period end must be in the future");
        return new BudgetPeriod(periodStart, periodEnd);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PeriodStart;
        yield return PeriodEnd;
    }
}