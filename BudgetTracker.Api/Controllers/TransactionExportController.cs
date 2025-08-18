using BudgetTracker.Application.Interfaces;
using BudgetTracker.Domain.Repositories.Filters;
using BudgetTracker.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker.Controllers;
[ApiController]
[Route("api/transaction/export")]
public class TransactionExportController : ControllerBase
{
    private Guid? UserId => User.GetUserId();
    private readonly ITransactionExportService _transactionExportService;

    public TransactionExportController(ITransactionExportService transactionExportService)
    {
        _transactionExportService = transactionExportService;
    }

    [HttpPost("csv")]
    public async Task<IActionResult> ExportTransactionsCsvAsync([FromQuery]TransactionFilter filter,
        CancellationToken cancellation)
    {
        if (UserId is null) return Unauthorized();
        string fileName = $"transactions_{DateTime.UtcNow:yyyyMMdd}.csv";
        string contentType = "text/csv";
        return new FileCallbackResult(contentType, async (output, _) =>
        {
            await _transactionExportService.ExportToCsvAsync(UserId.Value, filter, output, cancellation);
        })
        {
            FileDownloadName = fileName,
        };
    }
}