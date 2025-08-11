using BudgetTracker.Domain.Common.Exceptions;

namespace BudgetTracker.Domain.Models.User;

public sealed class RefreshToken
{
    
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string Token { get; private set; } = null!;
    public DateTime Expires { get;private set; }
    public bool Revoked { get;private set; }
    public DateTime Created { get;private set; }
    
    public static RefreshToken Create(Guid userId,Guid tokenId, string token, DateTime expires)
    {
        var refreshToken = new RefreshToken()
        {
            Id = tokenId,
            UserId = userId,
            Token = token,
            Expires = expires,
            Created = DateTime.UtcNow,
            Revoked = false,
        };
        return refreshToken;
    }

    public static void Revoke(RefreshToken refreshToken)
    {
        if (refreshToken.Revoked)
        {
            throw new RequestException("Token already revoked");
        }
        refreshToken.Revoked = true;
    }
}