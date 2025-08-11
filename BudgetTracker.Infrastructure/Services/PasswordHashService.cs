using BudgetTracker.Application.Interfaces;

namespace BudgetTracker.Infrastructure.Services;

public class PasswordHashService : IPasswordHashService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 10);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}