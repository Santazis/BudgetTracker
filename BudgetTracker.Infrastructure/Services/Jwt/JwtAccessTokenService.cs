using System.Security.Claims;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Infrastructure.Services.Jwt;

public class JwtAccessTokenService : IJwtAccessTokenService
{
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
    private readonly JwtSettings _jwtSettings;

    public JwtAccessTokenService(IJwtTokenGeneratorService jwtTokenGeneratorService, JwtSettings jwtSettings)
    {
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
        _jwtSettings = jwtSettings;
    }

    public string GenerateAccessToken(User user,Guid sessionId)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim("sessionId", sessionId.ToString()),
            new Claim("EmailVerified", user.IsEmailVerified.ToString())
        };
        return _jwtTokenGeneratorService.GenerateToken(_jwtSettings.AccessTokenSecret, _jwtSettings.Audience,
            _jwtSettings.Issuer, _jwtSettings.AccessTokenExpMinutes, claims);
    }
}