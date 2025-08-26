using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Budget;
using BudgetTracker.Application.Models.Budget.Requests;
using BudgetTracker.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly IBudgetService _budgetService;
    private readonly IValidator<CreateBudget> _createBudgetValidator;
    private readonly IValidator<UpdateBudget> _updateBudgetValidator;

    public BudgetController(IBudgetService budgetService, IValidator<CreateBudget> createBudgetValidator,
        IValidator<UpdateBudget> updateBudgetValidator)
    {
        _budgetService = budgetService;
        _createBudgetValidator = createBudgetValidator;
        _updateBudgetValidator = updateBudgetValidator;
    }

    [HttpPost]
    [ProducesResponseType<BudgetDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBudget request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var validate = await _createBudgetValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }

        var result = await _budgetService.CreateBudgetAsync(request, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
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
        var result = await _budgetService.GetBudgetById(budgetId, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateBudget request,
        CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var validate = await _updateBudgetValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }

        var result = await _budgetService.UpdateAsync(request, id, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _budgetService.DeleteAsync(id, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }
}