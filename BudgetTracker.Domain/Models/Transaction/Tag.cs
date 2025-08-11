namespace BudgetTracker.Domain.Models.Transaction;

public class Tag : Entity
{
    private Tag(Guid id) : base(id){}
    public string Name { get;private set; }
    public Guid? UserId { get;private set; }
    public User.User? User { get;private set; }
    public DateTime CreatedAt { get;private set; }
    public bool IsSystem { get;private set; }
    public static Tag Create(string name,bool isSystem, Guid? userId)
    {
        return new Tag(Guid.NewGuid())
        {
            Name = name,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsSystem = isSystem,
        };
    }
}