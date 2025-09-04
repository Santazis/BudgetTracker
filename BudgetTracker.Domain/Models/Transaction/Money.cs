using System.Text.Json.Serialization;

namespace BudgetTracker.Domain.Models.Transaction;

public sealed class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money() {}
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public static Money Create(decimal amount, string currency)
    {
        return new Money(amount, currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public static Money operator +(Money left, Money right)
    {
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static bool operator >(Money left, Money right)

    {
        if (left.Currency != right.Currency) Money.ConvertCurrency(left.Currency, right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        if (left.Currency != right.Currency) Money.ConvertCurrency(left.Currency, right);
        return left.Amount < right.Amount;
    }

    public static Money ConvertCurrency(string currency, Money money)
    {
        return Money.Create(money.Amount, currency);
    }
}