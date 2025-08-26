namespace BudgetTracker.Domain.Models.Transaction;

using Category;
using User;

public sealed class Transaction : Entity
{
    private Transaction(Guid id) : base(id)
    {
        _transactionTags = new List<TransactionTag>();
    }

    public Money Amount { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid? PaymentMethodId { get; private set; }
    public PaymentMethod? PaymentMethod { get; private set; }
    public string? Description { get; private set; }
    private List<TransactionTag> _transactionTags;
    public IReadOnlyCollection<TransactionTag> TransactionTags => _transactionTags;

    public static Transaction Create(Money amount,DateTime createdAt, Guid? paymentMethodId, Guid categoryId, Guid userId,
        string? description)
    {
        return new Transaction(Guid.NewGuid())
        {
            Amount = amount,
            CreatedAt = createdAt,
            Description = description,
            CategoryId = categoryId,
            UserId = userId,
            PaymentMethodId = paymentMethodId,
        };
    }

    public void Update(Money amount, string? description)
    {
        if (Amount == amount && Description == description) return;
        Amount = amount;
        Description = description;
    }

    public bool IsOwner(Guid userId)
    {
        return userId == UserId;
    }

    public void AttachPaymentMethod(Guid paymentMethodId)
    {
        if (PaymentMethodId == paymentMethodId) return;
        PaymentMethodId = paymentMethodId;
    }
    public void DetachPaymentMethod() => PaymentMethodId = null; 
    public void AttachTag(Guid tagId)
    {
        
        var existingTag = TransactionTags.FirstOrDefault(t => t.TransactionId == Id && t.TagId == tagId);
        if (existingTag is not null)
        {
            throw new ArgumentException("Tag already attached");
        }

        var transactionTag = new TransactionTag(transactionId: Id, tagId: tagId);
        _transactionTags.Add(transactionTag);
    }
    public void DetachTags(IEnumerable<Guid> tagIds)
    {
        _transactionTags.RemoveAll(t => tagIds.Contains(t.TagId));
    } 
}