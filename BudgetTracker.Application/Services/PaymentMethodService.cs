using BudgetTracker.Application.Interfaces;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PaymentMethodService(ITransactionRepository transactionRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> AttachPaymentMethodToTransactionAsync(Guid transactionId, Guid userId, Guid paymentMethodId,
        CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId, cancellation);
        if (transaction is null)
        {
            return Result.Failure(TransactionErrors.TransactionNotFound);
        }
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        var isPaymentMethodExists = user.HasPaymentMethod(paymentMethodId);
        if (!isPaymentMethodExists)
        {
            return Result.Failure(UserErrors.PaymentMethodNotFound);
        }
        transaction.AttachPaymentMethod(paymentMethodId);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }

    public async Task<Result> DetachPaymentMethodFromTransactionAsync(Guid transactionId, Guid userId, CancellationToken cancellation)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, userId, cancellation);
        if (transaction is null)
        {
            return Result.Failure(TransactionErrors.TransactionNotFound);
        }
        var user = await _userRepository.GetByIdAsync(userId, cancellation);
        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }
        transaction.DetachPaymentMethod();
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }
}