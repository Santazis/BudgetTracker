using BudgetTracker.Application.Models.RecurringTransaction.Requests;
using BudgetTracker.Domain.DomainServices;
using FluentValidation;

namespace BudgetTracker.Application.Validators.RecurringTransaction;

public class RecurringTransactionValidator : AbstractValidator<CreateRecurringTransaction>
{
    private readonly IRecurringCronScheduleCalculator _cron;
    public RecurringTransactionValidator(IRecurringCronScheduleCalculator cron)
    {
        _cron = cron;
        RuleFor(x=> x.Currency).NotEmpty().WithMessage("Currency is required");
        RuleFor(x=> x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x=> x.Description).MaximumLength(255).WithMessage("Description need to be less than 255 characters");
        RuleFor(x=> x.CronExpression).Must(IsValidCronExpression).WithMessage("Cron expression is not valid");
    }

     private bool IsValidCronExpression(string cronExpression)
     {
         return _cron.ValidateCronExpression(cronExpression);
     }
}


