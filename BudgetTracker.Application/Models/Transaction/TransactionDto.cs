using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Application.Models.Transaction;

public record TransactionDto(
    decimal Amount,
    string Currency ,
    string? Description,
    DateTime CreatedAt,
    CategoryDto Category,
    Guid TransactionId,
    PaymentMethodDto? PaymentMethod,
    IEnumerable<TagDto> Tags,
    Guid UserId)
{
    public static TransactionDto FromEntity(Domain.Models.Transaction.Transaction t) =>
        new TransactionDto(TransactionId: t.Id,
            Category: new CategoryDto(t.Category.Name, t.Category.Id, t.Category.Type.ToString()),
            Amount: t.Amount.Amount,
            Currency: t.Amount.Currency,
            UserId: t.UserId,
            Tags:t.TransactionTags.Select(t=> new TagDto(t.TagId,t.Tag.Name)),
            PaymentMethod: t.PaymentMethod is not null
                ? new PaymentMethodDto(t.PaymentMethodId!.Value, t.PaymentMethod.Name, t.PaymentMethod.Details,
                    t.PaymentMethod.Type.ToString())
                : null,
            Description: t.Description,
            CreatedAt: t.CreatedAt);
}