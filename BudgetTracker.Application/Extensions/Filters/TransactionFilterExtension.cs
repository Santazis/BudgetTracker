using BudgetTracker.Domain.Models.Enums;
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
            var date = filter.From.Value.Date;
            query = query.Where(t=> t.CreatedAt >= date);
        }

        if (filter.IsIncome.HasValue)
        {
            query = filter.IsIncome.Value ? query.Where(t=> t.Category.Type == CategoryTypes.Income) : query.Where(t=> t.Category.Type == CategoryTypes.Expense);       
        }
        if (filter.To.HasValue)
        {
            var date = filter.To.Value.Date.AddDays(1);
            query = query.Where(t=> t.CreatedAt <= date);
        }

        if (IsCollectionExistsAndNotEmpty(filter.Tags))
        {
            query = query.Where(t => t.TransactionTags.Any(tt => filter.Tags.Contains(tt.TagId)));
        }

        if (IsCollectionExistsAndNotEmpty(filter.PaymentMethods))
        {
            query = query.Where(t => t.PaymentMethodId != null && filter.PaymentMethods.Contains(t.PaymentMethodId.Value));
        }
        
        return query;
    }
    
    private static bool IsCollectionExistsAndNotEmpty(HashSet<Guid>? ids) =>  ids is not null && ids.Count > 0;
}
