using System.Security.Claims;

namespace BudgetTracker.Extensions;

public static class ClaimPrincipalExtension
{
    private const string UserIdClaim = "userId";

    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        if (user is null) return Guid.Empty;
        var claimValue = user.FindFirstValue(UserIdClaim);
        return Guid.TryParse(claimValue,out var userId) ? userId : null;;
    }
}