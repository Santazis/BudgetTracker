using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Extensions;

public static class TransactionSortingExtension
{
    public static IQueryable<Transaction> ApplySorting(this IQueryable<Transaction> query, TransactionFilter? sort)
    {
        if (sort is null)
        {
            return query.OrderByDescending(t => t.CreatedAt);
        }
        
        if (!string.IsNullOrWhiteSpace(sort.SortBy))
        {
            query = sort.SortBy switch
            {
                "date" => sort.Descending ? query.OrderByDescending(t=> t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                "amount" =>sort.Descending ? query.OrderByDescending(t=> t.Amount.Amount): query.OrderBy(t => t.Amount),
                "category" =>sort.Descending ? query.OrderByDescending(t=> t.Category.Id) : query.OrderBy(t => t.Category.Id),
                _ => sort.Descending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(t => t.CreatedAt);
        }
        return query;
    }
}