using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.RecurringTransaction;
using BudgetTracker.Application.Models.RecurringTransaction.Requests;
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
    public async Task<IActionResult> GetAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _recurringTransactionService.GetRecurringTransactionsAsync(UserId.Value, cancellation));
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
}