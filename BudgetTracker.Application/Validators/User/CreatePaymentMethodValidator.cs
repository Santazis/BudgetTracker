using BudgetTracker.Application.Models.User.Requests;
using FluentValidation;

namespace BudgetTracker.Application.Validators.User;

public class CreatePaymentMethodValidator : AbstractValidator<CreatePaymentMethod>
{
    public CreatePaymentMethodValidator( )
    {
        RuleFor(x=> x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100).WithMessage("Name must be less than 100 characters");
        RuleFor(x=> x.Details).MaximumLength(100).WithMessage("Details must be less than 100 characters");
        RuleFor(x => x.Type).Must(Enum.IsDefined).WithMessage("Invalid payment method type");
    }
}