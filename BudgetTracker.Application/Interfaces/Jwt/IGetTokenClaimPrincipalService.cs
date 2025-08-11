using System.Security.Claims;

namespace BudgetTracker.Application.Interfaces.Jwt;

public interface IGetTokenClaimPrincipalService
{
    ClaimsPrincipal GetPrincipalFromToken(string token);
}