using BudgetTracker.Application.Models.Category.Requests;
using FluentValidation;

namespace BudgetTracker.Application.Validators.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x=> x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(255).WithMessage("Name must be less than 255 characters");
        RuleFor(x => x.Type).Must(Enum.IsDefined).WithMessage("Invalid category type");
    }
}