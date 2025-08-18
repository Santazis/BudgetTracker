using Microsoft.AspNetCore.Mvc;

namespace BudgetTracker;

public class FileCallbackResult : FileResult
{
    private readonly Func<Stream, ActionContext, Task> _callback;

    public FileCallbackResult(string contentType, Func<Stream, ActionContext, Task> callback) 
        : base(contentType)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        if (!string.IsNullOrEmpty(FileDownloadName))
            response.Headers.Append("Content-Disposition", $"attachment; filename={FileDownloadName}");
        await _callback(response.Body, context);
    }
}