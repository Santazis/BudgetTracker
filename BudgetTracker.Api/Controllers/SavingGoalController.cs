using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.SavingGoal.Requests;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;
[ApiController]
[Route("api/goals")]
public class SavingGoalController :ControllerBase
{
    private readonly ISavingGoalService _savingGoalService;
    public Guid? UserId => User.GetUserId();
    public SavingGoalController(ISavingGoalService savingGoalService)
    {
        _savingGoalService = savingGoalService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSavingGoal request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _savingGoalService.CreateAsync(request, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType<SavingGoalDto>(200)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _savingGoalService.GetByIdAsync(id, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}