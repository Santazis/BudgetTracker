using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Interfaces.Auth;
using BudgetTracker.Application.Models.Auth;
using BudgetTracker.Application.Models.Auth.Requests;
using BudgetTracker.Application.Models.User.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUser request, CancellationToken cancellation)
    {
        await _authService.RegisterAsync(request, cancellation);
        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(200)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellation)
    {
        return Ok(await _authService.LoginAsync(request, cancellation));
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType<AuthResponse>(200)]
    public async Task<IActionResult> RefreshAsync([FromBody] string refreshToken, CancellationToken cancellation)
    {
        return Ok(await _authService.RefreshAsync(refreshToken, cancellation));
    }

    [HttpPost("session")]
    public async Task<IActionResult> GetSessionAsync(CancellationToken cancellation)
    {
        var username = User.Identity?.Name;
        if (username is null)
        {
            return Unauthorized();
        }
        return Ok(new {username});
    }
    [HttpPost("logout")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> LogoutAsync(
        CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        var sessionIdClaim = User.FindFirst("sessionId")?.Value;
        if (userIdClaim is null || sessionIdClaim is null)
        {
            return Unauthorized();
        }
        var userId = Guid.Parse(userIdClaim);
        var sessionId = Guid.Parse(sessionIdClaim);
        await _authService.LogoutAsync(userId, sessionId, cancellation);
        return Ok();
    }
}