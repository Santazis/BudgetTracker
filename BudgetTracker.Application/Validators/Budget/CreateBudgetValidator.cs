using BudgetTracker.Application.Models.Budget.Requests;
using FluentValidation;

namespace BudgetTracker.Application.Validators.Budget;

public class CreateBudgetValidator : AbstractValidator<CreateBudget>
{
    public CreateBudgetValidator()
    {
        RuleFor(x=> x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(255).WithMessage("Name must be less than 255 characters");
        RuleFor(x=> x.Description).MaximumLength(255).WithMessage("Description need to be less than 255 characters");
        RuleFor(x=> x.LimitAmount).GreaterThan(0).WithMessage("Limit amount must be greater than 0");
        RuleFor(x=> x.PeriodStart).GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Period start cannot be in past");
        RuleFor(x=> x.PeriodEnd).GreaterThan(x=> x.PeriodStart).WithMessage("Period end cannot be before period start");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Current is required");
    }
}