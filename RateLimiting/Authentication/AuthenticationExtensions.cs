using Microsoft.IdentityModel.Tokens;

namespace RateLimiting.Authentication;

public static class AuthenticationExtensions
{
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication().AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = "https://localhost:7041",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey("MySuperSecretSigningKey"u8.ToArray())
            };
        });

        return builder;
    }
}