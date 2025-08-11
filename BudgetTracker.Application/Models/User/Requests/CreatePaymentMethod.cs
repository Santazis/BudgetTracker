using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Application.Models.User.Requests;

public record CreatePaymentMethod(string Name, string? Details, PaymentMethodType Type);