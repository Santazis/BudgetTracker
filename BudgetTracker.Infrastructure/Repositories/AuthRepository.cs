using BudgetTracker.Domain.Models.User;
using BudgetTracker.Domain.Repositories;
using BudgetTracker.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BudgetTracker.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellation)
    {
        await _context.Users.AddAsync(user, cancellation);
        return user;
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellation)
    {
        var user = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellation);
        return user;
    }



    public async Task AddRefreshToken(RefreshToken token, CancellationToken cancellation)
    {
        await _context.RefreshTokens.AddAsync(token, cancellation);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(Guid userId, Guid refreshTokenId,
        CancellationToken cancellation)
    {
        var token = await _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == refreshTokenId, cancellation);
        return token;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellation)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellation);
        return user;
    }

    public void DeleteRefreshToken(RefreshToken token)
    {
      _context.RefreshTokens.Remove(token);
    }

    public async Task RemoveUserExpiredRefreshTokensAsync(Guid userId, CancellationToken cancellation)
    {
        var expiredTokens = await _context.RefreshTokens.Where(t => t.UserId == userId &&
            t.Expires < DateTime.UtcNow)
            .ToListAsync(cancellation);
        _context.RemoveRange(expiredTokens);
    }
}