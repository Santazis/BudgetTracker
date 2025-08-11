using System.Security.Claims;
using System.Text;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Domain.Models.User;
using Microsoft.IdentityModel.Tokens;

namespace BudgetTracker.Infrastructure.Services.Jwt;

public class JwtRefreshTokenService : IJwtRefreshTokenService
{
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
    private readonly JwtSettings _jwtSettings;

    public JwtRefreshTokenService(IJwtTokenGeneratorService jwtTokenGeneratorService, JwtSettings jwtSettings)
    {
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
        _jwtSettings = jwtSettings;
    }

    public string GenerateRefreshToken(User user,Guid tokenId)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Sid,tokenId.ToString()),
        };
        return _jwtTokenGeneratorService.GenerateToken(_jwtSettings.RefreshTokenSecret, _jwtSettings.Audience,
            _jwtSettings.Issuer, _jwtSettings.RefreshTokenExpMinutes, claims);
    }
}