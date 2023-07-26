using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using RateLimiting.Authentication;

namespace RateLimiting.User;

public static class UsersApi
{
    public static RouteGroupBuilder MapUsers(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/users");

        group.WithTags("Users");

        group.MapPost("/",
            async Task<Results<Ok, ValidationProblem>> (UserInfo newUser, UserManager<AppUser> userManager) =>
            {
                var result =
                    await userManager.CreateAsync(new AppUser { UserName = newUser.Username }, newUser.Password);

                if (result.Succeeded)
                    return TypedResults.Ok();
                return TypedResults.ValidationProblem(result.Errors.ToDictionary(e => e.Code,
                    e => new[] { e.Description }));
            });

        group.MapPost("/token",
            async Task<Results<BadRequest, Ok<AuthToken>>> (UserInfo userInfo, UserManager<AppUser> userManager,
                ITokenService tokenService) =>
            {
                var user = await userManager.FindByNameAsync(userInfo.Username);

                if (user is null || !await userManager.CheckPasswordAsync(user, userInfo.Password))
                    return TypedResults.BadRequest();

                return TypedResults.Ok(new AuthToken(tokenService.GenerateToken(user.UserName!)));
            });

        return group;
    }
}