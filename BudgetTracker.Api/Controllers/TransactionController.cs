using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Category;
using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Domain.Repositories.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ISummaryService _summaryService;
    
    public TransactionController(ITransactionService transactionService, ITransactionRepository transactionRepository, ISummaryService summaryService)
    {
        _transactionService = transactionService;
        _summaryService = summaryService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
    [HttpPost]
    [ProducesResponseType<TransactionDto>(200)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTransaction request, CancellationToken cancellation)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        return Ok(await _transactionService.CreateTransactionAsync(request,userId,cancellation));
    }
    [HttpPatch("{transactionId:guid}")]
    [ProducesResponseType<TransactionDto>(200)]
    public async Task<IActionResult> UpdateTransactionAsync([FromRoute] Guid transactionId,[FromBody] UpdateTransaction request, CancellationToken cancellation)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        return Ok(await _transactionService.UpdateTransactionAsync(transactionId,userId,request,cancellation));
    }
    [HttpDelete("{transactionId:guid}")]
    public async Task<IActionResult> DeleteTransactionAsync([FromRoute]Guid transactionId, CancellationToken cancellation)
    {

        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        await _transactionService.DeleteTransactionAsync(transactionId,userId,cancellation);
        return Ok();
    }
    [HttpGet]
    [ProducesResponseType<IEnumerable<TransactionDto>>(200)]
    public async Task<IActionResult> GetTransactionsByUserIdAsync(CancellationToken cancellation,[FromQuery] TransactionFilter? filter)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        return Ok(await _transactionService.GetTransactionsByUserIdAsync(userId,filter,cancellation));
    }
    
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryAsync([FromQuery] TransactionFilter? filter, CancellationToken cancellation)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        return Ok(await _summaryService.GetSummaryAsync(userId,filter,cancellation));
    }
    [HttpPost("{transactionId:guid}/attach/payment-method")]
    public async Task<IActionResult> AttachPaymentMethodAsync([FromRoute]Guid transactionId,[FromBody] Guid paymentMethodId, CancellationToken cancellation)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        await _transactionService.AttachPaymentMethodAsync(transactionId,userId,paymentMethodId,cancellation);
        return Ok();
    }
    
    [HttpPost("{transactionId:guid}/attach/tag")]
    public async Task<IActionResult> AttachTagAsync([FromRoute]Guid transactionId,[FromBody] Guid tagId, CancellationToken cancellation)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        await _transactionService.AttachTagAsync(transactionId,userId,tagId,cancellation);
        return Ok();
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAsync(IEnumerable<CreateTransaction> request, CancellationToken cancellation)
    {
        var userId = GetUserId();
        if (userId == Guid.Empty) return Unauthorized();
        await _transactionService.UploadMassTransactionsAsync(userId,request,cancellation);
        return Ok();
    }
}