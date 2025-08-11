namespace BudgetTracker.Application.Models.Auth;

public record AuthResponse(string AccessToken,string? RefreshToken);