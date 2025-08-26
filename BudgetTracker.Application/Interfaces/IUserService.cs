using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(Guid userId, CancellationToken cancellation);
    Task<Result> AddPaymentMethod(Guid userId, CreatePaymentMethod request, CancellationToken cancellation);
    Task<Result> DeletePaymentMethodAsync(Guid userId, Guid paymentMethodId, CancellationToken cancellation);
    Task<Result> UpdatePaymentMethodAsync(Guid userId, Guid paymentMethodId, UpdatePaymentMethod request,
        CancellationToken cancellation);
    Task<IEnumerable<Tag>> GetUserTagsAsync(Guid userId, CancellationToken cancellation);
}