using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Models.Auth.Requests;

public record LoginRequest(string Email, string Password);
