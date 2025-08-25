namespace BudgetTracker.Domain.Common.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = new("InvalidCredentials","Invalid credentials");
    public static readonly Error UserNotFound = new("UserNotFound","User not found");
    public static readonly Error UserAlreadyExist = new("UserAlreadyExist","User already exist");
    public static readonly Error InvalidToken = new("InvalidToken","Invalid token");
}