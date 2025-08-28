using System.Linq.Expressions;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Domain.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter?  filter,PaginationRequest request,CancellationToken cancellation);
    IAsyncEnumerable<Transaction> GetTransactionsAsync(TransactionFilter? filter, Guid userId,
        CancellationToken cancellation);
    Task<Transaction> CreateAsync(Transaction transaction,CancellationToken cancellation);
    Task<Transaction?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    Task<List<Transaction>> GetAllAsync(Guid userId,TransactionFilter? filter,CancellationToken cancellation);
    Task<decimal> GetSpentAmountAsync(Guid userId,TransactionFilter? filter,CancellationToken cancellation);
    Task<Dictionary<Guid,decimal>> GetCategoriesSpentAmountAsync(Guid userId,TransactionFilter? filter,CancellationToken cancellation);
    void Delete(Transaction transaction);
    Task CreateRangeAsync(IEnumerable<Transaction> transactions,CancellationToken cancellation);
    Task<int> CountAsync(Guid userId,TransactionFilter? filter, CancellationToken cancellation);
}