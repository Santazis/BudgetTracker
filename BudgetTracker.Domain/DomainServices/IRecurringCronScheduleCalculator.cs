using BudgetTracker.Domain.Models.RecurringTransaction;

namespace BudgetTracker.Domain.DomainServices;

public interface IRecurringCronScheduleCalculator
{
    DateTime CalculateNextRunDate(RecurringTransaction transaction);
    DateTime CalculateRunDate(string cronExpression);
}