using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Application.Models.Budget.Requests;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly IBudgetService _budgetService;
    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpPost]
    [ProducesResponseType<BudgetDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBudget request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _budgetService.CreateBudgetAsync(request, UserId.Value, cancellation));
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<BudgetDto>>(200)]
    public async Task<IActionResult> GetActiveBudgetsAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _budgetService.GetActiveBudgetsAsync(UserId.Value, cancellation));
    }
    
    [HttpGet("{budgetId:guid}")]
    [ProducesResponseType<BudgetDto>(200)]
    public async Task<IActionResult> GetBudgetByIdAsync([FromRoute] Guid budgetId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _budgetService.GetBudgetById(budgetId, UserId.Value, cancellation));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,[FromBody]UpdateBudget request,CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _budgetService.UpdateAsync(request,id,UserId.Value,cancellation);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _budgetService.DeleteAsync(id,UserId.Value,cancellation);
        return Ok();
    }
}