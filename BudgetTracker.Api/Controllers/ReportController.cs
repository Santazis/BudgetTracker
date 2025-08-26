using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Domain.Repositories.Filters;
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
    [ProducesResponseType<SummaryDto>(200)]
    [HttpGet("transaction-summary")]
    public async Task<IActionResult> GetSummaryAsync([FromQuery] TransactionFilter? filter, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _summaryService.GetSummaryAsync(UserId.Value, filter, cancellation));
    }
}