using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter? filter,PaginationRequest request, CancellationToken cancellation);
    Task<Result<TransactionDto>> CreateTransactionAsync(CreateTransaction request,Guid userId, CancellationToken cancellation);
    Task<Result> DeleteTransactionAsync(Guid transactionId,Guid userId, CancellationToken cancellation);
    Task<Result<TransactionDto>> UpdateTransactionAsync(Guid transactionId,Guid userId, UpdateTransaction request,
        CancellationToken cancellation);
    Task UploadMassTransactionsAsync(Guid userId, IEnumerable<CreateTransaction> request, CancellationToken cancellation);
}