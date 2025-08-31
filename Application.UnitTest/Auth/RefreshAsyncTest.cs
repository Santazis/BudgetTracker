using System.Security.Claims;
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Application.Services.Auth;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.User;
using BudgetTracker.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest.Auth;

public class RefreshAsyncTest
{
    private static readonly string refreshToken = "refresh-token";
    private readonly IAuthRepository _authRepositoryMock;
    private readonly IPasswordHashService _passwordHashServiceMock;
    private readonly IJwtAccessTokenService _jwtAccessTokenServiceMock;
    private readonly IJwtRefreshTokenService _jwtRefreshTokenServiceMock;
    private readonly JwtSettings _jwtSettingsMock;
    private readonly IGetTokenClaimPrincipalService _getTokenClaimPrincipalServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IAuthService _authService;

    public RefreshAsyncTest()
    {
        _authRepositoryMock = Substitute.For<IAuthRepository>();
        _passwordHashServiceMock = Substitute.For<IPasswordHashService>();
        _jwtAccessTokenServiceMock = Substitute.For<IJwtAccessTokenService>();
        _jwtRefreshTokenServiceMock = Substitute.For<IJwtRefreshTokenService>();
        _jwtSettingsMock = new JwtSettings(); // Assuming this has a parameterless constructor
        _getTokenClaimPrincipalServiceMock = Substitute.For<IGetTokenClaimPrincipalService>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _authService = new AuthService(
            authRepository: _authRepositoryMock,
            passwordHashService: _passwordHashServiceMock,
            jwtAccessTokenService: _jwtAccessTokenServiceMock,
            jwtRefreshTokenService: _jwtRefreshTokenServiceMock,
            jwtSettings: _jwtSettingsMock,
            getTokenClaimPrincipalService: _getTokenClaimPrincipalServiceMock,
            unitOfWork: _unitOfWorkMock);
    }

    [Fact]
    public async Task RefreshAsync_Should_ReturnError_WhenTokenIsInvalid()
    {
        //arrange
        var invalidRefreshToken = "invalid-refresh-token";
        _getTokenClaimPrincipalServiceMock.GetPrincipalFromToken(invalidRefreshToken)
            .Returns(Result<ClaimsPrincipal>.Failure(AuthErrors.InvalidToken));
        //act
        var result = await _authService.RefreshAsync(invalidRefreshToken, CancellationToken.None);
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.InvalidToken);
    }

    [Fact]
    public async Task RefreshAsync_Should_ReturnError_WhenTokenEntityNotFound()
    {
        //arrange
        var userId = Guid.NewGuid();
        var refreshTokenId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new Claim("userId", userId.ToString()),
            new Claim(ClaimTypes.Sid, refreshTokenId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "mock");
        var principal = new ClaimsPrincipal(identity);
        _getTokenClaimPrincipalServiceMock.GetPrincipalFromToken(refreshToken)
            .Returns(Result<ClaimsPrincipal>.Success(principal));

        _authRepositoryMock.GetRefreshTokenAsync(Arg.Is<Guid>(id => id == userId),
                Arg.Is<Guid>(r => r == refreshTokenId), Arg.Any<CancellationToken>())
            .Returns((RefreshToken?)null);
        //act
        var result = await _authService.RefreshAsync(refreshToken, CancellationToken.None);
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.RefreshTokenNotFound);
    }

    [Fact]
    public async Task RefreshAsync_Should_ReturnSuccess_WhenTokenValidAndEntityFound()
    {
        //arrange
        var userId = Guid.NewGuid();
        var refreshTokenId = Guid.NewGuid();
        var claims = new List<Claim>    
        {
            new Claim("userId", userId.ToString()),
            new Claim(ClaimTypes.Sid, refreshTokenId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "mock");
        var principal = new ClaimsPrincipal(identity);
        var refreshTokenEntity = RefreshToken.Create(userId, refreshTokenId,refreshToken,DateTime.UtcNow.AddDays(1));
        var user = User.Create(Name.Create("Firstname", "Lastname"), "username", "hashedpassword",
            new Email("test@email.com"));
        _getTokenClaimPrincipalServiceMock.GetPrincipalFromToken(refreshToken)
            .Returns(Result<ClaimsPrincipal>.Success(principal));

        _authRepositoryMock.GetRefreshTokenAsync(userId, refreshTokenId, Arg.Any<CancellationToken>())
            .Returns(refreshTokenEntity);
        
        _authRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == userId), Arg.Any<CancellationToken>())
            .Returns(user);
        _jwtAccessTokenServiceMock.GenerateAccessToken(user, Arg.Any<Guid>())
            .Returns("mock-access-token");

        _jwtRefreshTokenServiceMock.GenerateRefreshToken(user, Arg.Any<Guid>())
            .Returns("mock-refresh-token");

        _authRepositoryMock.DeleteRefreshToken(Arg.Any<RefreshToken>());
        _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // act
        var result = await _authService.RefreshAsync(refreshToken, CancellationToken.None);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("mock-access-token");
        result.Value.RefreshToken.Should().Be("mock-refresh-token");
    }
}