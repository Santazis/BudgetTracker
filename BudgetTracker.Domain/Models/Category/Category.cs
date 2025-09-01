using BudgetTracker.Domain.Models.Enums;

namespace BudgetTracker.Domain.Models.Category;

public sealed class Category : Entity
{
    private Category(Guid id) : base(id){}
    
    public bool IsSystem { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public CategoryTypes Type { get; private set; }
    public Guid? UserId { get; private set; }
    public User.User? User { get; private set; }
    public static Category Create(Guid userId, string name, CategoryTypes type)
    {
        return new Category(Guid.NewGuid())
        {
            IsSystem = false,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            Type = type,
            UserId = userId,
        };
    }

    public void Update(string name,CategoryTypes type)
    {
        if(name == Name) return;
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        Name = name;
        Type = type;
    }
 }
