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

    public async Task<Tag> CreateAsync(Tag tag, CancellationToken cancellation)
    {
        await _context.Tags.AddAsync(tag, cancellation);
        return tag;
    }

    public async Task<Tag?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellation)
    {
        var tag =await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellation);
        return tag;
    }

    public async Task<int> CountAsync(Guid userId, CancellationToken cancellation)
    {
        var tags =await _context.Tags.Where(t => t.UserId == userId).CountAsync(cancellation);
        return tags;
    }

    public void Delete(Tag tag)
    {
        _context.Tags.Remove(tag);
    }
}