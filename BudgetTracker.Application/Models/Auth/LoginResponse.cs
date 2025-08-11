using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Models.Auth;

public record LoginResponse(AuthResponse Auth,string Email,bool IsEmailVerified);