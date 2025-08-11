
namespace BudgetTracker.Application.Models.Transaction;

public class TransactionCsvImport
{
    
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}