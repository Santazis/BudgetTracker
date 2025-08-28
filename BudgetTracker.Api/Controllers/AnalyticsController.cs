using BudgetTracker.Application.Interfaces.Analytics;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;
[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IBudgetAnalyticService _analyticService;
    public Guid? UserId => User.GetUserId();

    public AnalyticsController(IBudgetAnalyticService analyticService)
    {
        _analyticService = analyticService;
    }

    [HttpGet("budget-forecast/{budgetId:guid}")]
    public async Task<IActionResult> GetBudgetForecastAsync(Guid budgetId,CancellationToken cancellation)
    {
        if(UserId is null) return Unauthorized();
        var result = await _analyticService.GetForecastAsync(UserId.Value,budgetId,cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}