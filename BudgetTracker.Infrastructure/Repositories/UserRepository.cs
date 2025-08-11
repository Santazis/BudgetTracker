using BudgetTracker.Domain.Models.User;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellation)
    {
        var user = await _context.Users
            .Include(u=> u.PaymentMethods)
            .FirstOrDefaultAsync(u=> u.Id == id, cancellation);
        return user;
    }
}