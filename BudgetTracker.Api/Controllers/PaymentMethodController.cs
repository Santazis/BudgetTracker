using BudgetTracker.Application.Interfaces;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;
[ApiController]
[Route("api/payment-method")]
public class PaymentMethodController:ControllerBase
{
    public Guid? UserId => User.GetUserId();
    private readonly IPaymentMethodService _paymentMethodService;

    public PaymentMethodController(IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }
        
    [HttpPost("attach/{transactionId:guid}")]
    public async Task<IActionResult> AttachPaymentMethodAsync([FromRoute]Guid transactionId,[FromBody] Guid paymentMethodId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result =  await _paymentMethodService.AttachPaymentMethodToTransactionAsync(transactionId, UserId.Value, paymentMethodId, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);       
        }
        return Ok();
    }
 
    [HttpDelete("detach/{transactionId:guid}")]
    public async Task<IActionResult> DetachPaymentMethodAsync([FromRoute]Guid transactionId,[FromBody] Guid paymentMethodId, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _paymentMethodService.DetachPaymentMethodFromTransactionAsync(transactionId, UserId.Value, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);      
        }
        return Ok();
    }
}