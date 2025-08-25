using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Models.User;

namespace BudgetTracker.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Result<RegisterResponse>> RegisterAsync(CreateUser request, CancellationToken cancellation);
    Task<Result<AuthResponse>> AuthAsync(User user, CancellationToken cancellation);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellation);
    Task<Result> LogoutAsync(Guid userId,Guid sessionId,CancellationToken cancellation);
    Task<Result<AuthResponse>> RefreshAsync(string refreshToken,CancellationToken cancellation);
}
