using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Extensions.Filters;

public static class TransactionFilterExtension
{
    public static IQueryable<Transaction> Filter(this IQueryable<Transaction> query, TransactionFilter? filter)
    {
        if (filter is null)
        {
            return query;
        }

        if (filter.Categories is not null && filter.Categories.Count > 0)
        {
            query = query.Where(t=> filter.Categories.Contains(t.CategoryId));
        }
        if (filter.From.HasValue)
        {
            query = query.Where(t=> t.CreatedAt >= filter.From.Value);
        }
        if (filter.To.HasValue)
        {

            query = query.Where(t=> t.CreatedAt <= filter.To.Value);
        }

        if (filter.Tags is not null && filter.Tags.Count > 0)
        {
            query = query.Where(t => t.TransactionTags.Any(tt => filter.Tags.Contains(tt.TagId)));
        }

        if (filter.PaymentMethods is not null && filter.PaymentMethods.Count > 0)
        {
            query = query.Where(t => t.PaymentMethodId != null && filter.PaymentMethods.Contains(t.PaymentMethodId.Value));
        }
        return query;
    }
}