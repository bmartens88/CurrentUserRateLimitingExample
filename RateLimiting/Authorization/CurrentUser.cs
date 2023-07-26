using System.Security.Claims;
using RateLimiting.User;

namespace RateLimiting.Authorization;

public sealed class CurrentUser
{
    public AppUser? User { get; set; }
    public ClaimsPrincipal Principal { get; set; } = default!;

    public string Id => Principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
}