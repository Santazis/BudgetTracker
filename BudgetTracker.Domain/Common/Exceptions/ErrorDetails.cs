using System.Text.Json;

namespace BudgetTracker.Domain.Common.Exceptions;

public class ErrorDetails
{
    public required string Message { get; set; }

    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}