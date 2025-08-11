using BudgetTracker.Domain.Models.Transaction;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tag>> GetUserTagsAsync(Guid userId, CancellationToken cancellation)
    {
        var tags = await _context.Tags.AsNoTracking()
            .Where(t=> t.UserId == userId || t.IsSystem)
            .ToListAsync(cancellation);
        return tags;
    }
}