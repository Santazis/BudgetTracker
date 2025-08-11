using System.Linq.Expressions;
using BudgetTracker.Domain.Models.Budget;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Domain.Repositories;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter?  filter,CancellationToken cancellation);
    Task<Transaction> CreateAsync(Transaction transaction,CancellationToken cancellation);
    Task<Transaction?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    Task<List<Transaction>> GetTransactionsForBudgetAsync(Guid userId,Guid categoryId,BudgetPeriod period,CancellationToken cancellation);
    void Delete(Transaction transaction);
    Task CreateRangeAsync(IEnumerable<Transaction> transactions,CancellationToken cancellation);
}