using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Repositories.Filters;

namespace BudgetTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Guid userId,TransactionFilter? filter,PaginationRequest request, CancellationToken cancellation);
    Task<TransactionDto> CreateTransactionAsync(CreateTransaction request,Guid userId, CancellationToken cancellation);
    Task DeleteTransactionAsync(Guid transactionId,Guid userId, CancellationToken cancellation);
    Task<TransactionDto> UpdateTransactionAsync(Guid transactionId,Guid userId, UpdateTransaction request,
        CancellationToken cancellation);
    Task AttachPaymentMethodAsync(Guid transactionId,Guid userId, Guid paymentMethodId, CancellationToken cancellation);
    Task AttachTagAsync(Guid transactionId,Guid userId, Guid tagId, CancellationToken cancellation);
    Task DetachTagsAsync(Guid transactionId,Guid userId,IEnumerable<Guid> tagIds, CancellationToken cancellation);
    Task DetachPaymentMethodAsync(Guid transactionId,Guid userId, Guid paymentMethodId, CancellationToken cancellation);
    Task UploadMassTransactionsAsync(Guid userId, IEnumerable<CreateTransaction> request, CancellationToken cancellation);
}