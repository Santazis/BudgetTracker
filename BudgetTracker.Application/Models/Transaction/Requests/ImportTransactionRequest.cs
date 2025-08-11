namespace BudgetTracker.Application.Models.Transaction.Requests;

public record ImportTransactionRequest(
    int NameIndex,
    int AmountIndex,
    int DateIndex,
    int DescriptionIndex,
    bool IsFirstRowHeaders
);