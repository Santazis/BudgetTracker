using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces;

public interface ITagService
{
    Task<Result> AttachTagsToTransactionAsync(Guid transactionId,Guid userId,List<Guid> tagIds,CancellationToken cancellation);
    Task<Result> DetachTagsFromTransactionAsync(Guid transactionId,Guid userId,List<Guid> tagIds,CancellationToken cancellation);
}