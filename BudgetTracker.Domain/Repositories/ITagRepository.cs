using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Repositories;

public interface ITagRepository
{
    Task<List<Tag>> GetUserTagsAsync(Guid userId,CancellationToken cancellation);
    Task<Tag> CreateAsync(Tag tag,CancellationToken cancellation);
    Task<Tag?> GetByIdAsync(Guid id,Guid userId,CancellationToken cancellation);
    Task<int> CountAsync(Guid userId,CancellationToken cancellation);
    void Delete(Tag tag);
}