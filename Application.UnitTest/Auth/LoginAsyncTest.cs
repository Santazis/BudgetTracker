using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Application.Services.Auth;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.User;
using BudgetTracker.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest.Auth;

public class LoginAsyncTest
{
    private static readonly LoginRequest request = new("login@email.com","password123456");
    private readonly IAuthRepository _authRepositoryMock;
    private readonly IPasswordHashService _passwordHashServiceMock;
    private readonly IJwtAccessTokenService _jwtAccessTokenServiceMock;
    private readonly IJwtRefreshTokenService _jwtRefreshTokenServiceMock;
    private readonly JwtSettings _jwtSettingsMock;
    private readonly IGetTokenClaimPrincipalService _getTokenClaimPrincipalServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IAuthService _authService;
    
    public LoginAsyncTest()
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
    public async Task LoginAsync_Should_ReturnError_WhenEmailIncorrect()
    {
        //arrange
        var email = new Email(request.Email);
        _authRepositoryMock.GetByEmailAsync(Arg.Is<Email>(e => e.Value == email.Value), Arg.Any<CancellationToken>())
            .Returns((User?)null);
        
        //act
        var result = await _authService.LoginAsync(request, CancellationToken.None);
        
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.InvalidCredentials);
    }

    [Fact]
    public async Task LoginAsync_Should_ReturnError_WhenPasswordIncorrect()
    {
        //arrange
        var user = User.Create(Name.Create("Firstname","Lastname"), "username", "hashedpassword", new Email(request.Email));
        _authRepositoryMock.GetByEmailAsync(Arg.Is<Email>(e => e.Value == request.Email), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHashServiceMock.VerifyPassword(request.Password, user.PasswordHash).Returns(false);
        //act
        var result = await _authService.LoginAsync(request, CancellationToken.None);
        
        //assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.InvalidCredentials);
    }
    
    [Fact]
    public async Task LoginAsync_Should_ReturnSuccess_WhenEmailAndPasswordCorrect()
    {
        //arrange
        var user = User.Create(Name.Create("Firstname","Lastname"), "username", "hashedpassword", new Email(request.Email));
        _authRepositoryMock.GetByEmailAsync(Arg.Is<Email>(e => e.Value == request.Email), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHashServiceMock.VerifyPassword(request.Password, user.PasswordHash).Returns(true);
        _jwtAccessTokenServiceMock.GenerateAccessToken(Arg.Any<User>(), Arg.Any<Guid>())
            .Returns("mock-access-token");
    
        _jwtRefreshTokenServiceMock.GenerateRefreshToken(Arg.Any<User>(), Arg.Any<Guid>())
            .Returns("mock-refresh-token");
        
        _authRepositoryMock.RemoveUserExpiredRefreshTokensAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
    
        _authRepositoryMock.AddRefreshToken(Arg.Any<RefreshToken>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
    
        _unitOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        //act
        var result = await _authService.LoginAsync(request, CancellationToken.None);
        //assert
        
        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("mock-access-token");
        result.Value.RefreshToken.Should().Be("mock-refresh-token");
        result.Value.Email.Should().Be(request.Email);
        result.Value.IsEmailVerified.Should().Be(user.IsEmailVerified);

        
    }
}