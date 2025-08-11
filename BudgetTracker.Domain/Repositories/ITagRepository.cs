using BudgetTracker.Domain.Models.Transaction;

namespace BudgetTracker.Domain.Repositories;

public interface ITagRepository
{
    Task<List<Tag>> GetUserTagsAsync(Guid userId,CancellationToken cancellation);
}