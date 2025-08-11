using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Application.Models.Budget.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    
    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpPost]
    [ProducesResponseType<BudgetDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBudget request, CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _budgetService.CreateBudgetAsync(request,userId,cancellation));
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<BudgetDto>>(200)]
    public async Task<IActionResult> GetActiveBudgetsAsync(CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _budgetService.GetActiveBudgetsAsync(userId,cancellation));
    }
    [HttpGet("{budgetId:guid}")]
    [ProducesResponseType<BudgetDto>(200)]
    public async Task<IActionResult> GetBudgetByIdAsync([FromRoute] Guid budgetId, CancellationToken cancellation)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (userIdClaim is null) return Unauthorized();
        var userId = Guid.Parse(userIdClaim);
        return Ok(await _budgetService.GetBudgetById(budgetId,userId,cancellation));
    }
}