namespace BudgetTracker.Application.Models.Transaction.Requests;

public record UpdateTransaction(
    string? Description,
    decimal Amount,
    string Currency);