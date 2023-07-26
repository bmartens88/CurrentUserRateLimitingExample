using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RateLimiting;
using RateLimiting.Authentication;
using RateLimiting.Authorization;
using RateLimiting.Extensions;
using RateLimiting.Todos;
using RateLimiting.User;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthentication();
builder.Services.AddAuthorizationBuilder().AddCurrentUserHandler();

builder.Services.AddTokenService();

var connectionString = builder.Configuration.GetConnectionString("Todos") ?? "Data Source = :memory:;Cache=Shared";
await using var connection = new SqliteConnection(connectionString);
await connection.OpenAsync();

builder.Services.AddDbContext<AppDbContext>(opts => opts.UseSqlite(connection));

builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCurrentUser();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRateLimiting();

var app = builder.Build();

app.UseRateLimiter();

app.MapUsers();
app.MapTodos();

app.Run();