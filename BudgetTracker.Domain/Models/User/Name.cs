namespace BudgetTracker.Domain.Models.User;

public sealed class Name : ValueObject
{
    public string Firstname { get;  }
    public string Lastname { get;  }

    private Name(string firstname, string lastname)
    {
        Firstname = firstname;
        Lastname = lastname; 
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Firstname;
        yield return Lastname;  
    }

    public static Name Create(string firstname, string lastname)
    {
        if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        return new Name(firstname, lastname);
    }
}