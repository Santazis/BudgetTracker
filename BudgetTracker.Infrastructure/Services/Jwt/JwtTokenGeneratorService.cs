using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetTracker.Application.Interfaces.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace BudgetTracker.Infrastructure.Services.Jwt;

public class JwtTokenGeneratorService : IJwtTokenGeneratorService
{
    public string GenerateToken(string key, string audience, string issuer, double expires,
        IEnumerable<Claim>? claims = null)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new JwtSecurityToken(issuer, audience, claims, notBefore: DateTime.UtcNow,
            expires: DateTime.Now.AddMinutes(expires), signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}