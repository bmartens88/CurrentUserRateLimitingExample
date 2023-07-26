using RateLimiting.Authorization;
using RateLimiting.Extensions;

namespace RateLimiting.Todos;

public static class TodoApi
{
    public static RouteGroupBuilder MapTodos(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/todos");

        group.WithTags("Todos");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        group.RequirePerUserRateLimit();

        group.MapGet("/", (CurrentUser owner) =>
        {
            return TypedResults.Ok(owner);
        });
        
        return group;
    }
}