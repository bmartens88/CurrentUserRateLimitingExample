using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace RateLimiting.Authentication;

public static class AuthenticationServiceExtensions
{
    public static IServiceCollection AddTokenService(this IServiceCollection services)
    {
        return services.AddSingleton<ITokenService, TokenService>();
    }
}

public interface ITokenService
{
    string GenerateToken(string username);
}

public sealed class TokenService : ITokenService
{
    private readonly string _issuer;
    private readonly SigningCredentials _jwtSigningCredentials;

    public TokenService(IAuthenticationConfigurationProvider authenticationConfigurationProvider)
    {
        var bearerSection =
            authenticationConfigurationProvider.GetSchemeConfiguration(JwtBearerDefaults.AuthenticationScheme);
        _issuer = bearerSection["ValidIssuer"] ?? throw new InvalidOperationException("Issuer is not specified");
        var signingKeyBytes = "MySuperSecretSigningKey"u8.ToArray();
        _jwtSigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKeyBytes),
            SecurityAlgorithms.HmacSha256Signature);
    }

    public string GenerateToken(string username)
    {
        var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, username));

        var id = Guid.NewGuid().ToString().GetHashCode().ToString("x", CultureInfo.InvariantCulture);

        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, id));

        var handler = new JwtSecurityTokenHandler();

        var jwtToken = handler.CreateJwtSecurityToken(
            _issuer,
            null,
            identity,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(30),
            DateTime.UtcNow,
            _jwtSigningCredentials);

        return handler.WriteToken(jwtToken);
    }
}