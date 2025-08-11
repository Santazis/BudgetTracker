namespace BudgetTracker.Domain.Common.Exceptions;

public class RequestException : Exception
{

    public RequestException(string message) : base(message)
    {

    }
}