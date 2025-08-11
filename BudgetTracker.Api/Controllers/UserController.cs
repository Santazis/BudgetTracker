using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Application.Models.User.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITransactionImportService _transactionImportService;

    public UserController(IUserService userService, ITransactionImportService transactionImportService)
    {
        _userService = userService;
        _transactionImportService = transactionImportService;
    }

    [HttpGet]
    [ProducesResponseType<UserDto>(200)]
    public async Task<IActionResult> GetByIdAsync(CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _userService.GetByIdAsync(userId, cancellation));
    }

    [HttpPost("payment-method")]
    public async Task<IActionResult> AddPaymentMethodAsync([FromBody] CreatePaymentMethod request,
        CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        await _userService.AddPaymentMethod(userId, request, cancellation);
        return Ok();
    }

    [HttpPatch("payment-method/{paymentMethodId:guid}/update")]
    public async Task<IActionResult> UpdatePaymentMethod([FromBody] UpdatePaymentMethod request,
        [FromRoute] Guid paymentMethodId, CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        await _userService.UpdatePaymentMethodAsync(userId, paymentMethodId, request, cancellation);
        return Ok();
    }

    [HttpDelete("payment-method/{paymentMethodId:guid}")]
    public async Task<IActionResult> DeletePaymentMethod([FromRoute] Guid paymentMethodId,
        CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        await _userService.DeletePaymentMethodAsync(userId, paymentMethodId, cancellation);
        return Ok();
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetUserTagsAsync(CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _userService.GetUserTagsAsync(userId, cancellation));
    }

    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportFromCsvAsync(IFormFile csv, CancellationToken cancellation)
    {
        if (csv.Length > 5 * 1024 * 1024)
        {
            return BadRequest("File is too large");
        }

        if (!csv.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Invalid file type. Only .csv is supported.");
        await using var stream = csv.OpenReadStream();
        
        return Ok(await _transactionImportService.ImportFromCsvAsync(stream, Guid.Empty, cancellation));
    }


}