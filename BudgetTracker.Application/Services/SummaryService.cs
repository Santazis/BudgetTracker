using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Filters;
using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Models.Enums;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Services;

public class SummaryService : ISummaryService
{
    private readonly ITransactionRepository _transactionRepository;

    public SummaryService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<SummaryDto> GetSummaryAsync(Guid userId, TransactionFilter? filter,
        CancellationToken cancellation)
    {
        var transactions = await _transactionRepository.GetAllAsync(userId, filter, cancellation);
        if (transactions.Count == 0)
        {
            var money = Money.Create(0, "USD");
            return  new SummaryDto(
                Total: money,
                TotalIncome: money,
                TotalExpense: money,
                ByCategory: []);
        }

        var categories = new Dictionary<Guid,(Money total,CategoryDto category)>();
        var totalIncome = Money.Create(0, "USD");
        var totalExpense = Money.Create(0, "USD");
        foreach (var transaction in transactions)
        {
            if (!categories.TryGetValue(transaction.CategoryId, out var category))
            {
                categories[transaction.CategoryId] = (transaction.Amount, new CategoryDto(transaction.Category.Name, transaction.Category.Id, transaction.Category.Type.ToString()));
            }
            else
            {
                categories[transaction.CategoryId] = (category.total + transaction.Amount, category.category);
            }
            if (transaction.Category.Type == CategoryTypes.Income)
            {
                totalIncome += transaction.Amount;
            }
            else
            {
                totalExpense += transaction.Amount;
            }
            
        }
        var totalBalance = totalIncome - totalExpense;
        var byCategory = categories.Values.Select(v=> new CategorySummary(v.category,v.total))
                .OrderBy(c=> c.Category.Type)
                .ToList();
        var summary = new SummaryDto(
            Total: totalBalance,
            TotalIncome: totalIncome,
            TotalExpense: totalExpense,
            ByCategory: byCategory);
        return summary;
    }
}