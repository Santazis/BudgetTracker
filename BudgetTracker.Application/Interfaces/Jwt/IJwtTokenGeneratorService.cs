using System.Security.Claims;

namespace BudgetTracker.Application.Interfaces.Jwt;

public interface IJwtTokenGeneratorService
{
    string GenerateToken(string key,string audience,string issuer,double expires,IEnumerable<Claim>? claims = null);
}