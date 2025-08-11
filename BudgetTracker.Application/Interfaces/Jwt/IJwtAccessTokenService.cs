using System.Security.Claims;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Interfaces.Jwt;

public interface IJwtAccessTokenService
{
    string GenerateAccessToken(User user,Guid sessionId);
}