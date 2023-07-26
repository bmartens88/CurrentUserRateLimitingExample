using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RateLimiting.User;

public sealed class AppUser : IdentityUser
{
}

public sealed class UserInfo
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}