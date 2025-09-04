using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Domain.Models.User;

public sealed class User : Entity
{
    private User(Guid id) : base(id)
    {
        _paymentMethods = new List<PaymentMethod>();
    }
    
    private User() {}
    public Name Name { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string Username { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public Email Email { get; private set; } = null!;
    public bool IsEmailVerified { get; private set; }
    private List<PaymentMethod> _paymentMethods;
    public IReadOnlyCollection<PaymentMethod> PaymentMethods => _paymentMethods;
    public static User Create(Name name,string username, string passwordHash,Email email)
    {
        return new User(Guid.NewGuid())
        {
            Name = name,
            Username = username,
            IsEmailVerified = false,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            Email = email, 
        };
    }
    public void AddPaymentMethod(PaymentMethodType type,string name,string? details)
    {
       if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
       if (_paymentMethods.Any(m=> string.Equals(m.Name,name,StringComparison.OrdinalIgnoreCase)))
       {
           throw new ArgumentException("Payment method already exists");
       }
       
       var paymentMethod = new PaymentMethod(id:Guid.NewGuid(), userId:Id,type:type,name:name,details:details);
       _paymentMethods.Add(paymentMethod);
    }

    public void UpdatePaymentMethod(Guid id,string name, string? details, PaymentMethodType type)
    {
        var paymentMethod = _paymentMethods.FirstOrDefault(m=>m.Id == id);
        if (paymentMethod is null)
        {
            throw new ArgumentException("Payment method does not exist");
        }
        paymentMethod.Name = name;
        paymentMethod.Details = details;
        paymentMethod.Type = type;
    }

    public bool HasPaymentMethod(Guid id)
    {
        return _paymentMethods.Any(m=>m.Id == id);
    }
    public void DeletePaymentMethod(Guid id)
    {
        var paymentMethod = _paymentMethods.FirstOrDefault(m=>m.Id == id);
        if (paymentMethod is null)
        {
            throw new ArgumentException("Payment method does not exist");
        }
        _paymentMethods.Remove(paymentMethod);
    }
}


public sealed class Email : ValueObject
{
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public Email(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        if(!IsValidEmail(value)) throw new ArgumentException("Email is not valid");
        Value = value;
    }
    
    
    
     private static bool IsValidEmail(string value)
 {
     return Regex.IsMatch(value,
         @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
 }
 public static implicit operator string(Email email) => email.Value;
}