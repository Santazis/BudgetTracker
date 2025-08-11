using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(CreateUser request, CancellationToken cancellation);
    Task<AuthResponse> AuthAsync(User user, CancellationToken cancellation);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellation);
    Task LogoutAsync(Guid userId,Guid sessionId,CancellationToken cancellation);
    Task<AuthResponse> RefreshAsync(string refreshToken,CancellationToken cancellation);
}
