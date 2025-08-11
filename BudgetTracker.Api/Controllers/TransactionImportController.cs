using BudgetTracker.Application.Interfaces;
using BudgetTracker.Application.Models.Transaction.Requests;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;

[ApiController]
[Route("api/transaction/import")]
public class TransactionImportController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ITransactionImportService _transactionImportService;

    public TransactionImportController(ITransactionImportService transactionImportService)
    {
        _transactionImportService = transactionImportService;
    }
    
    [HttpPost("import-csv")]
    public async Task<IActionResult> ImportFromCsvAsync(IFormFile csv,[FromForm] ImportTransactionRequest request, CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        if (csv.Length > 5 * 1024 * 1024)
        {
            return BadRequest("File is too large");
        }

        if (!csv.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Invalid file type. Only .csv is supported.");
        
        await using var stream = csv.OpenReadStream();
        return Ok(await _transactionImportService.ImportFromCsvAsync(stream, UserId.Value,request, cancellation));
    }
}