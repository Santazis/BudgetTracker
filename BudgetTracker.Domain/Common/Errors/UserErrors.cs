namespace BudgetTracker.Domain.Common.Errors;

public static class UserErrors
{
    public static readonly Error PaymentMethodNotFound = new("User.PaymentMethodNotFound","Payment method not found");
    public static readonly Error UserNotFound = new("User.NotFound","User not found");

}