using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Interfaces.Jwt;

public interface IJwtRefreshTokenService
{
    string GenerateRefreshToken(User user,Guid tokenId);
}