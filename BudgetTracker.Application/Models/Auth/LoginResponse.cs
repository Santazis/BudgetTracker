namespace BudgetTracker.Application.Models.Auth;

public record LoginResponse(string AccessToken,string? RefreshToken,string Email,bool IsEmailVerified);