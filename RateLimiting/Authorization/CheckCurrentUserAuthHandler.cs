using Microsoft.AspNetCore.Authorization;

namespace RateLimiting.Authorization;

public static class AuthorizationHandlerExtensions
{
    public static AuthorizationBuilder AddCurrentUserHandler(this AuthorizationBuilder builder)
    {
        builder.Services.AddScoped<IAuthorizationHandler, CheckCurrentUserAuthHandler>();
        return builder;
    }

    public static AuthorizationPolicyBuilder RequireCurrentUser(this AuthorizationPolicyBuilder builder)
    {
        return builder.RequireAuthenticatedUser()
            .AddRequirements(new CheckCurrentUserRequirement());
    }

    private class CheckCurrentUserRequirement : IAuthorizationRequirement
    {
    }

    private sealed class CheckCurrentUserAuthHandler : AuthorizationHandler<CheckCurrentUserRequirement>
    {
        private readonly CurrentUser _currentUser;

        public CheckCurrentUserAuthHandler(CurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            CheckCurrentUserRequirement requirement)
        {
            if (_currentUser.User is not null)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}