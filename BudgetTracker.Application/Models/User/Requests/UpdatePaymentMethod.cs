using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Application.Models.User.Requests;

public record UpdatePaymentMethod(
    string Name,
    string? Details,
    PaymentMethodType Type
);