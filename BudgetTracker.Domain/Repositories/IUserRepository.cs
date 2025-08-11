using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id,CancellationToken cancellation);
}