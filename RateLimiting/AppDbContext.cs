﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RateLimiting.User;

namespace RateLimiting;

public sealed class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options) :
        base(options)
    {
        Database.EnsureCreated();
    }
}