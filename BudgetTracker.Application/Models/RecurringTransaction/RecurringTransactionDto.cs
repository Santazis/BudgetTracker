using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.RecurringTransaction;

public record RecurringTransactionDto(
    Guid Id,
    Guid UserId,
    CategoryDto Category,
    PaymentMethodDto? PaymentMethod,
    IEnumerable<TagDto> Tags,
    string? Description,
    Money Amount,
    string CronExpression,
    DateTime CreatedAt,
    bool IsActive,
    DateTime? NextRun,
    DateTime? LastRun)
{
    public static RecurringTransactionDto FromEntity(Domain.Models.RecurringTransaction.RecurringTransaction t) =>
        new RecurringTransactionDto(
            Id: t.Id,
            UserId: t.UserId,
            Category: new CategoryDto(t.Category.Name, t.Category.Id, t.Category.Type.ToString()),
            PaymentMethod: t.PaymentMethod is not null
                ? new PaymentMethodDto(t.PaymentMethodId!.Value, t.PaymentMethod.Name, t.PaymentMethod.Details,
                    t.PaymentMethod.Type.ToString())
                : null,
            Tags: t.RecurringTransactionTags.Select(rt => new TagDto(rt.TagId, rt.Tag.Name)),
            Description: t.Description,
            Amount: t.Amount,
            CronExpression: t.CronExpression,
            CreatedAt: t.CreatedAt,
            IsActive: t.IsActive,
            NextRun: t.NextRun,
            LastRun: t.LastRun);
}