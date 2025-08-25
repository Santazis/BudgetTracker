using BudgetTracker.Application.Interfaces;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ISummaryService _summaryService;

    public ReportController(ISummaryService summaryService)
    {
        _summaryService = summaryService;
    }
    [HttpGet("top-expenses-month")]
    public async Task<IActionResult> GetTopExpensesCategoriesInMonthAsync(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _summaryService.GetTopExpensesCategoriesInMonthAsync(UserId.Value, cancellation));
    }
    [HttpGet("comparison-month")]
    public async Task<IActionResult> GetComparison(CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _summaryService.GetMonthlySpendingComparisonAsync(UserId.Value, cancellation));
    }
}