using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellation);
    Task AddPaymentMethod(Guid userId, CreatePaymentMethod request, CancellationToken cancellation);
    Task DeletePaymentMethodAsync(Guid userId, Guid paymentMethodId, CancellationToken cancellation);
    Task UpdatePaymentMethodAsync(Guid userId, Guid paymentMethodId, UpdatePaymentMethod request,
        CancellationToken cancellation);
    Task<IEnumerable<Tag>> GetUserTagsAsync(Guid userId, CancellationToken cancellation);
}