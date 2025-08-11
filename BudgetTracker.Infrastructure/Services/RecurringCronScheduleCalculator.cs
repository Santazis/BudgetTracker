using BudgetTracker.Domain.DomainServices;
using BudgetTracker.Domain.Models.RecurringTransaction;
using NCrontab;

namespace BudgetTracker.Infrastructure.Services;

public class RecurringCronScheduleCalculator : IRecurringCronScheduleCalculator
{
    public DateTime CalculateRunDate(string cronExpression)
    {
        var time = DateTime.UtcNow;
        var schedule = CrontabSchedule.Parse(cronExpression);
        var day = ParseDayFromCron(cronExpression);
        var next = schedule.GetNextOccurrence(time);
        var daysInMonth = DateTime.DaysInMonth(time.Year, next.Month);

        if (day.HasValue && day.Value > daysInMonth)
        {
            var lastDayOfMonth = day.Value - daysInMonth;
            next = next.AddDays(-lastDayOfMonth);
        }
        
        return next;
    }
    

    public DateTime CalculateNextRunDate(RecurringTransaction transaction)
    {
        var time = DateTime.UtcNow;
        var schedule = CrontabSchedule.Parse(transaction.CronExpression);
        var day = ParseDayFromCron(transaction.CronExpression);
        var next = schedule.GetNextOccurrence(time);
        var daysInMonth = DateTime.DaysInMonth(time.Year, next.Month);

        if (day.HasValue && day.Value > daysInMonth)
        {
            var lastDayOfMonth = day.Value - daysInMonth;
            next = next.AddDays(-lastDayOfMonth);
        }
        
        return next;
    }

    private int? ParseDayFromCron(string cronExp)
    {
        if (string.IsNullOrWhiteSpace(cronExp))
        {
            return null;
        }
        var parts = cronExp.Split(' ');
        if (parts.Length < 5)
        {
            return null;
        }
        var day = parts[2];
        if (day == "*")
        {
            return null;
        }
        return int.TryParse(day,out var dayInt) ? dayInt : null;
    }
}