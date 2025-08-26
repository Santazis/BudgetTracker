using BudgetTracker.Application.Interfaces;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;
[ApiController]
[Route("api/tag")]
public class TagController:ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost("attach/{transactionId:guid}")]
    public async Task<IActionResult> AttachTagAsync([FromRoute]Guid transactionId,[FromBody] List<Guid> tagIds, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _tagService.AttachTagsToTransactionAsync(transactionId, UserId.Value, tagIds, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }
    
    [HttpDelete("detach/{transactionId:guid}")]
    public async Task<IActionResult> DetachTagAsync([FromRoute]Guid transactionId,[FromBody] List<Guid> tagIds, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        var result = await _tagService.DetachTagsFromTransactionAsync(transactionId, UserId.Value, tagIds, cancellation);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }
}