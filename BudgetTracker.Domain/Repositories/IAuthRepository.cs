using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Domain.Repositories;

public interface IAuthRepository
{
   Task<User> CreateAsync(User user,CancellationToken cancellation);
   Task<User?> GetByEmailAsync(Email email,CancellationToken cancellation);
   Task<User?> GetByIdAsync(Guid userId,CancellationToken cancellation);
   Task AddRefreshToken(RefreshToken token,CancellationToken cancellation);
   Task<RefreshToken?> GetRefreshTokenAsync(Guid userId,Guid refreshTokenId,CancellationToken cancellation);
   void DeleteRefreshToken(RefreshToken token);
   Task RemoveUserExpiredRefreshTokensAsync(Guid userId,CancellationToken cancellation);
}