
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Domain.Common.Pagination;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;
using BudgetTracker.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ITransactionService _transactionService;
    private readonly IValidator<CreateTransaction> _createTransactionValidator;
    private readonly IValidator<UpdateTransaction> _updateTransactionValidator;
    public TransactionController(ITransactionService transactionService,   IValidator<CreateTransaction> createTransactionValidator, IValidator<UpdateTransaction> updateTransactionValidator)
    {
        _transactionService = transactionService;
        _createTransactionValidator = createTransactionValidator;
        _updateTransactionValidator = updateTransactionValidator;
    }
    
    [HttpPost]
    [ProducesResponseType<TransactionDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTransaction request, CancellationToken cancellation)
    {
        var validate = await _createTransactionValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }
        if (UserId is null) return Unauthorized();
        var result = await _transactionService.CreateTransactionAsync(request, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }
    
    [HttpPatch("{transactionId:guid}")]
    [ProducesResponseType<TransactionDto>(200)]
    public async Task<IActionResult> UpdateTransactionAsync([FromRoute] Guid transactionId,[FromBody] UpdateTransaction request, CancellationToken cancellation)
    {
        var validate = await _updateTransactionValidator.ValidateAsync(request, cancellation);
        if (!validate.IsValid)
        {
            return BadRequest(validate.ToDictionary());
        }
        if (UserId is null) return Unauthorized();
        var result = await _transactionService.UpdateTransactionAsync(transactionId, UserId.Value, request, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);       
        }
        return Ok(result.Value);
    }
    
    [HttpDelete("{transactionId:guid}")]
    public async Task<IActionResult> DeleteTransactionAsync([FromRoute]Guid transactionId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _transactionService.DeleteTransactionAsync(transactionId, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);       
        }
        return Ok();
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<TransactionDto>>(200)]
    public async Task<IActionResult> GetTransactionsByUserIdAsync(CancellationToken cancellation,
        [FromQuery] TransactionFilter? filter,
        [FromQuery]PaginationRequest request)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _transactionService.GetTransactionsByUserIdAsync(UserId.Value, filter,request, cancellation));
    }
    [HttpGet("count")]
    [ProducesResponseType<int>(200)]
    public async Task<IActionResult> GetTransactionsCountAsync(CancellationToken cancellation,
        [FromQuery] TransactionFilter? filter)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _transactionService.CountAsync(UserId.Value, filter, cancellation));
    }

    
}