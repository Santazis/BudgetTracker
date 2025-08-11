using BudgetTracker.Domain.Models.Category;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellation)
    {
        await _context.Categories.AddAsync(category, cancellation);
        await _context.SaveChangesAsync(cancellation);
        return category;
    }

    public async Task<List<Category>> GetUserCategoriesAsync(Guid userId, CancellationToken cancellation)
    {
        var categories = await _context.Categories.Where(c => c.UserId == userId || c.IsSystem).ToListAsync(cancellation);
        return categories;
    }

    public async Task<Category?> GetByIdAsync(Guid id,Guid userId, CancellationToken cancellation)
    {
        var category = await _context.Categories
            .Where(c=> c.Id == id  && (c.IsSystem || c.UserId == id) )
            .FirstOrDefaultAsync(c => c.Id == id , cancellation);
        return category;
    }

    public void DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
    }
    
}