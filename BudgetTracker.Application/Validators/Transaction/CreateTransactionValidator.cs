using BudgetTracker.Application.Models.Transaction.Requests;
using FluentValidation;

namespace BudgetTracker.Application.Validators.Transaction;

public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Description).MaximumLength(255).WithMessage("Description need to be less than 255 characters");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required");
    }
}
