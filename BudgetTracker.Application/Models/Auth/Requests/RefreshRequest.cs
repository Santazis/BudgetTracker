namespace BudgetTracker.Application.Models.Auth.Requests;

public class RefreshRequest
{
    public string RefreshToken { get; set; } = null!;
}