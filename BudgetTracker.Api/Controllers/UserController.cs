using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Application.Models.User;
using BudgetTracker.Application.Models.User.Requests;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
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
        if (UserId is null) return Unauthorized();
        return Ok(await _userService.GetByIdAsync(UserId.Value, cancellation));
    }

    [HttpPost("payment-method")]
    public async Task<IActionResult> AddPaymentMethodAsync([FromBody] CreatePaymentMethod request,
        CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _userService.AddPaymentMethod(UserId.Value, request, cancellation);
        return Ok();
    }

    [HttpPatch("payment-method/{paymentMethodId:guid}/update")]
    public async Task<IActionResult> UpdatePaymentMethod([FromBody] UpdatePaymentMethod request,
        [FromRoute] Guid paymentMethodId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _userService.UpdatePaymentMethodAsync(UserId.Value, paymentMethodId, request, cancellation);
        return Ok();
    }

    [HttpDelete("payment-method/{paymentMethodId:guid}")]
    public async Task<IActionResult> DeletePaymentMethod([FromRoute] Guid paymentMethodId,
        CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _userService.DeletePaymentMethodAsync(UserId.Value, paymentMethodId, cancellation);
        return Ok();
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetUserTagsAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _userService.GetUserTagsAsync(UserId.Value, cancellation));
    }


}