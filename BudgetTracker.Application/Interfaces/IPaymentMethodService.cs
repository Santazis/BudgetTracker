using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces;

public interface IPaymentMethodService
{
    Task<Result> AttachPaymentMethodToTransactionAsync(Guid transactionId,Guid userId,Guid paymentMethodId,CancellationToken cancellation);
    Task<Result> DetachPaymentMethodFromTransactionAsync(Guid transactionId,Guid userId,CancellationToken cancellation);
}