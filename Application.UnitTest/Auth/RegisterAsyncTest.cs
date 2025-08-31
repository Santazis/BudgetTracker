using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Interfaces.Jwt;
using BudgetTracker.Application.Models.Jwt;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Application.Services.Auth;
using BudgetTracker.Domain.Common.Errors;
using BudgetTracker.Domain.Models.User;
using BudgetTracker.Domain.Repositories;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest.Auth;

public class RegisterAsyncTest
{
    private static readonly CreateUser request = new(Firstname:"Firstname",Lastname:"Lastname",Email:"test@email.com",Password:"password123456",Username:"Username");
    private readonly IAuthRepository _authRepositoryMock;
    private readonly IPasswordHashService _passwordHashServiceMock;
    private readonly IJwtAccessTokenService _jwtAccessTokenServiceMock;
    private readonly IJwtRefreshTokenService _jwtRefreshTokenServiceMock;
    private readonly JwtSettings _jwtSettingsMock;
    private readonly IGetTokenClaimPrincipalService _getTokenClaimPrincipalServiceMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IAuthService _authService;
    
    public RegisterAsyncTest()
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
    public async Task RegisterAsync_Should_ReturnError_WhenEmailAlreadyExist()
    {
        // Arrange
        var existingUser = User.Create(
            Name.Create(request.Firstname,request.Lastname),
            "existinguser",
            "hashedpassword",
            new Email(request.Email));
        
        _authRepositoryMock.GetByEmailAsync(
            Arg.Is<Email>(e => e.Value == existingUser.Email), 
            Arg.Any<CancellationToken>())
            .Returns(existingUser);

        // Act
        var result = await _authService.RegisterAsync(request, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AuthErrors.UserAlreadyExist);
    }
    
    [Fact]
    public async Task RegisterAsync_Should_ReturnSuccess_WhenEmailUnique()
    {
        // Arrange
        
        
        _authRepositoryMock.GetByEmailAsync(
                Arg.Is<Email>(e => e.Value == request.Email), 
                Arg.Any<CancellationToken>())
            .Returns((User?)null);

        var createdUser = User.Create(Name.Create(request.Firstname, request.Lastname), request.Username,
            request.Password, new Email(request.Email));
        // Act
        _authRepositoryMock.CreateAsync(Arg.Is<User>(u => u.Email.Value == request.Email), Arg.Any<CancellationToken>())
            .Returns(createdUser);
        
        var result = await _authService.RegisterAsync(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(request.Email);
    }
}