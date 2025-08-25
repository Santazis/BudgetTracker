using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Application.Validators.Auth;
using BudgetTracker.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly IAuthService _authService;
    private readonly IValidator<CreateUser> _createUserValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;

    public AuthController(IAuthService authService, IValidator<CreateUser> createUserValidator,
        IValidator<LoginRequest> loginRequestValidator)
    {
        _authService = authService;
        _createUserValidator = createUserValidator;
        _loginRequestValidator = loginRequestValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUser request, CancellationToken cancellation)
    {
        var validate = await _createUserValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }

        var result = await _authService.RegisterAsync(request, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(200)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellation)
    {
        var validate = await _loginRequestValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }

        var result = await _authService.LoginAsync(request, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    [ProducesResponseType<AuthResponse>(200)]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest refreshToken,
        CancellationToken cancellation)
    {
        var result = await _authService.RefreshAsync(refreshToken.RefreshToken, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpGet("session")]
    public async Task<IActionResult> GetSessionAsync(CancellationToken cancellation)
    {
        var username = User.Identity?.Name;
        if (username is null)
        {
            return Unauthorized();
        }

        return Ok(new { username });
    }

    [HttpPost("logout")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();

        var sessionIdClaim = User.FindFirst("sessionId")?.Value;
        if (sessionIdClaim is null) return Unauthorized();

        var sessionId = Guid.Parse(sessionIdClaim);
        var result = await _authService.LogoutAsync(UserId.Value, sessionId, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);       
        }
        return Ok();
    }
}