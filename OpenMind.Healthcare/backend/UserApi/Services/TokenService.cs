using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserApi.Domain;
using UserApi.Domain.Aggregates;

namespace UserApi.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(Guid userId, string ipAddress);
}

public class TokenService(IConfiguration configuration) : ITokenService
{
    private int AccessTokenExpirationMinutes => configuration.GetValue("Jwt:AccessTokenExpirationMinutes", 15);
    private int RefreshTokenExpirationDays => configuration.GetValue("Jwt:RefreshTokenExpirationDays", 7);

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(Guid userId, string ipAddress)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        var token = Convert.ToBase64String(randomBytes);

        return RefreshToken.Create(userId, token, RefreshTokenExpirationDays, ipAddress);
    }
}
