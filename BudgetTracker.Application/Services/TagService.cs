using BudgetTracker.Application.Interfaces;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services;

public class TagService : ITagService
{
     private readonly ITransactionRepository _transactionRepository;
     private readonly IUnitOfWork _unitOfWork;
     public TagService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
     {
          _transactionRepository = transactionRepository;
          _unitOfWork = unitOfWork;
     }

     public async Task<Result> AttachTagsToTransactionAsync(Guid transactionId, Guid userId, List<Guid> tagIds, CancellationToken cancellation)
     {
          var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId, cancellation);
          if (transaction is null)
          {
               return Result.Failure(TransactionErrors.TransactionNotFound);
          }

          tagIds.ForEach(t=> transaction.AttachTag(t));
          await _unitOfWork.SaveChangesAsync(cancellation);
          return Result.Success;
     }

     public async Task<Result> DetachTagsFromTransactionAsync(Guid transactionId, Guid userId, List<Guid> tagIds, CancellationToken cancellation)
     {
          var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId, cancellation);
          if (transaction is null)
          {
               return Result.Failure(TransactionErrors.TransactionNotFound);
          }
          transaction.DetachTags(tagIds);
          await _unitOfWork.SaveChangesAsync(cancellation);
          return Result.Success;
     }
}