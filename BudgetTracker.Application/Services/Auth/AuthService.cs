using System.Security.Claims;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Common.Exceptions;
using BudgetTracker.Domain.Models.User;
using BudgetTracker.Domain.Repositories;

namespace BudgetTracker.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtAccessTokenService _jwtAccessTokenService;
    private readonly IJwtRefreshTokenService _jwtRefreshTokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly IGetTokenClaimPrincipalService _getTokenClaimPrincipalService;
    private readonly IUnitOfWork _unitOfWork;
    public AuthService(IAuthRepository authRepository, IPasswordHashService passwordHashService, IJwtAccessTokenService jwtAccessTokenService, IJwtRefreshTokenService jwtRefreshTokenService, JwtSettings jwtSettings, IGetTokenClaimPrincipalService getTokenClaimPrincipalService, IUnitOfWork unitOfWork)
    {
        _authRepository = authRepository;
        _passwordHashService = passwordHashService;
        _jwtAccessTokenService = jwtAccessTokenService;
        _jwtRefreshTokenService = jwtRefreshTokenService;
        _jwtSettings = jwtSettings;
        _getTokenClaimPrincipalService = getTokenClaimPrincipalService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisterResponse>> RegisterAsync(CreateUser request, CancellationToken cancellation)
    {
        var email = new Email(request.Email);
        var isUserExist = await _authRepository.GetByEmailAsync(email,cancellation) is not null;
        if (isUserExist)
        {
            return Result<RegisterResponse>.Failure(AuthErrors.UserAlreadyExist);
        }
        var userFullName = Name.Create(request.Firstname, request.Lastname);
        var user = Domain.Models.User.User.Create(userFullName, request.Username, _passwordHashService.HashPassword(request.Password), email);
        var createdUser =  await _authRepository.CreateAsync(user,cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        var registerResponse = new RegisterResponse()
        {
            Email = createdUser.Email,
        };
        return Result<RegisterResponse>.Success(registerResponse); 
    }

    public async Task<Result<AuthResponse>> AuthAsync(Domain.Models.User.User user, CancellationToken cancellation)
    {
        var refreshTokenId = Guid.NewGuid();
        var accessToken = _jwtAccessTokenService.GenerateAccessToken(user,refreshTokenId);
        var refreshToken = _jwtRefreshTokenService.GenerateRefreshToken(user,refreshTokenId);
        RefreshToken token = RefreshToken.Create(user.Id,
            refreshTokenId,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpMinutes));
        await _authRepository.RemoveUserExpiredRefreshTokensAsync(user.Id,cancellation);
        await _authRepository.AddRefreshToken(token,cancellation);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result<AuthResponse>.Success(new AuthResponse(accessToken,refreshToken));
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellation)
    {
        var user = await _authRepository.GetByEmailAsync(new Email(request.Email),cancellation);
        if (user is null)
        {
            return Result<LoginResponse>.Failure(AuthErrors.InvalidCredentials);
        }

        if (!_passwordHashService.VerifyPassword(request.Password,user.PasswordHash))
        {
            return Result<LoginResponse>.Failure(AuthErrors.InvalidCredentials);
        }
        Result<AuthResponse> authResponse = await AuthAsync(user,cancellation);
        if (authResponse.IsFailure)
        {
           return Result<LoginResponse>.Failure(authResponse.Error);
        }
        return Result<LoginResponse>.Success(new LoginResponse(authResponse.Value,user.Email,user.IsEmailVerified));
    }



    public async Task<Result<AuthResponse>> RefreshAsync(string refreshToken,CancellationToken cancellation)
    {
        var tokenPrincipals = _getTokenClaimPrincipalService.GetPrincipalFromToken(refreshToken);
        var userId = tokenPrincipals.Claims.First(c => "userId" == c.Type).Value;
        var tokenId = tokenPrincipals.Claims.First(c => ClaimTypes.Sid == c.Type).Value;
        var refreshTokenId = Guid.Parse(tokenId);
        var userGuid = Guid.Parse(userId);
        var refreshTokenEntity = await _authRepository.GetRefreshTokenAsync(userGuid,refreshTokenId,cancellation);
        if (refreshTokenEntity is null)
        {
            return Result<AuthResponse>.Failure(AuthErrors.InvalidCredentials);
        }
        var user = await _authRepository.GetByIdAsync(userGuid,cancellation);
        if (user is null)
        { return Result<AuthResponse>.Failure(AuthErrors.UserNotFound);

        }
        var authResponse = await AuthAsync(user,cancellation);
        if (authResponse.IsFailure)
        {
            return Result<AuthResponse>.Failure(authResponse.Error);
        }
        _authRepository.DeleteRefreshToken(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return authResponse;
    }

    public async Task<Result> LogoutAsync(Guid userId, Guid sessionId, CancellationToken cancellation)
    {
        var refreshToken = await _authRepository.GetRefreshTokenAsync(userId,sessionId,cancellation);
        if (refreshToken is null)
        {
          return  Result.Failure(AuthErrors.InvalidToken);
        }
        _authRepository.DeleteRefreshToken(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellation);
        return Result.Success;
    }
}