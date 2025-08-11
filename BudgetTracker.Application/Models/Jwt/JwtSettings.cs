namespace BudgetTracker.Application.Models.Jwt;

public class JwtSettings
{
    public string AccessTokenSecret { get; set; } = null!;
    public string RefreshTokenSecret { get; set; } = null!;
    public double AccessTokenExpMinutes { get; set; }
    public double RefreshTokenExpMinutes { get; set; }
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
}