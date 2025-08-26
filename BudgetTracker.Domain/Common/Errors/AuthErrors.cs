namespace BudgetTracker.Domain.Common.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = new("Auth.InvalidCredentials","Invalid credentials");
    public static readonly Error UserAlreadyExist = new("Auth.UserAlreadyExist","User already exist");
    public static readonly Error InvalidToken = new("Auth.InvalidToken","Invalid token");
    public static readonly Error RefreshTokenNotFound = new("Auth.TokenNotFound","Token not found");
}