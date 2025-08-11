using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Domain.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace BudgetTracker.Infrastructure.Services.Jwt;

public class GetTokenClaimPrincipalService : IGetTokenClaimPrincipalService
{
    private readonly JwtSettings _jwtSettings;

    public GetTokenClaimPrincipalService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret)),
            ValidateLifetime = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new RequestException("Invalid token");
        }
        return principal;
    }
}