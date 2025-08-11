
using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ITransactionService _transactionService;
    private readonly ISummaryService _summaryService;
    
    public TransactionController(ITransactionService transactionService, ITransactionRepository transactionRepository, ISummaryService summaryService)
    {
        _transactionService = transactionService;
        _summaryService = summaryService;
    }
    
    [HttpPost]
    [ProducesResponseType<TransactionDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTransaction request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _transactionService.CreateTransactionAsync(request, UserId.Value, cancellation));
    }
    
    [HttpPatch("{transactionId:guid}")]
    [ProducesResponseType<TransactionDto>(200)]
    public async Task<IActionResult> UpdateTransactionAsync([FromRoute] Guid transactionId,[FromBody] UpdateTransaction request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _transactionService.UpdateTransactionAsync(transactionId, UserId.Value, request, cancellation));
    }
    
    [HttpDelete("{transactionId:guid}")]
    public async Task<IActionResult> DeleteTransactionAsync([FromRoute]Guid transactionId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _transactionService.DeleteTransactionAsync(transactionId, UserId.Value, cancellation);
        return Ok();
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<TransactionDto>>(200)]
    public async Task<IActionResult> GetTransactionsByUserIdAsync(CancellationToken cancellation,[FromQuery] TransactionFilter? filter)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _transactionService.GetTransactionsByUserIdAsync(UserId.Value, filter, cancellation));
    }
    
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryAsync([FromQuery] TransactionFilter? filter, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        return Ok(await _summaryService.GetSummaryAsync(UserId.Value, filter, cancellation));
    }
    
    [HttpPost("{transactionId:guid}/attach/payment-method")]
    public async Task<IActionResult> AttachPaymentMethodAsync([FromRoute]Guid transactionId,[FromBody] Guid paymentMethodId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _transactionService.AttachPaymentMethodAsync(transactionId, UserId.Value, paymentMethodId, cancellation);
        return Ok();
    }
    
    [HttpPost("{transactionId:guid}/attach/tag")]
    public async Task<IActionResult> AttachTagAsync([FromRoute]Guid transactionId,[FromBody] Guid tagId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        await _transactionService.AttachTagAsync(transactionId, UserId.Value, tagId, cancellation);
        return Ok();
    }
    

}