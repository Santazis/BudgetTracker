using BudgetTracker.Application.Models.User.Requests;
using FluentValidation;

namespace BudgetTracker.Application.Validators.Auth;

public class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
        RuleFor(x=> x.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required").MinimumLength(5).WithMessage("Username must be at least 5 characters long");
        RuleFor(x=> x.Firstname).NotEmpty().WithMessage("Firstname is required");
        RuleFor(x=> x.Lastname).NotEmpty().WithMessage("Lastname is required");
        
    }
}