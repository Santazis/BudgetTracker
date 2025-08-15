using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class RecurringTransactionController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly IRecurringTransactionService _recurringTransactionService;

    public RecurringTransactionController(IRecurringTransactionService recurringTransactionService)
    {
        _recurringTransactionService = recurringTransactionService;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<RecurringTransactionDto>>(200)]
    public async Task<IActionResult> GetByUserIdAsync([FromQuery]PaginationRequest request,CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _recurringTransactionService.GetByUserIdAsync(UserId.Value, request,cancellation));
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRecurringTransaction request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _recurringTransactionService.CreateRecurringTransactionAsync(request, UserId.Value, cancellation);
        return Ok();
    }
    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> CreateFromRecAsync([FromRoute] Guid id, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _recurringTransactionService.CreateTransactionFromRecurringTransactionAsync(id, UserId.Value, cancellation);
        return Ok();
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,CancellationToken cancellation)
    {
        var userId = User.GetUserId();
        if (userId is null) return Unauthorized();
        await _recurringTransactionService.DeleteAsync(id,userId.Value,cancellation);
        return Ok();
    }   
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,[FromBody] UpdateRecurringTransaction request,CancellationToken cancellation)
    {
        var userId = User.GetUserId();
        if (userId is null) return Unauthorized();
        await _recurringTransactionService.UpdateAsync(id,userId.Value,request,cancellation);
        return Ok();   
    }
    
}