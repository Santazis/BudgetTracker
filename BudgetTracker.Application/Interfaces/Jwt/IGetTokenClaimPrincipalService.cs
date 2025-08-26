using System.Security.Claims;
using BudgetTracker.Domain.Common;

namespace BudgetTracker.Application.Interfaces.Jwt;

public interface IGetTokenClaimPrincipalService
{
    Result<ClaimsPrincipal> GetPrincipalFromToken(string token);
}