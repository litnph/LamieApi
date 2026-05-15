using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Lamie.Application.Common.Auth;
using Lamie.Domain.Entities.Auth;
using Lamie.Shared.Time;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lamie.Infrastructure.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;
    private readonly IClock _clock;
    private readonly SigningCredentials _credentials;

    public JwtTokenService(IOptions<JwtOptions> options, IClock clock)
    {
        _options = options.Value;
        _clock = clock;

        if (string.IsNullOrWhiteSpace(_options.SecretKey) || Encoding.UTF8.GetByteCount(_options.SecretKey) < 32)
        {
            throw new InvalidOperationException(
                "Jwt:SecretKey must be configured with at least 32 bytes (256 bits) of entropy.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public AccessTokenResult IssueAccessToken(User user)
    {
        var now = _clock.UtcNow;
        var expires = now.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("full_name", user.FullName),
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: _credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new AccessTokenResult(jwt, expires);
    }

    public RefreshTokenResult IssueRefreshToken()
    {
        Span<byte> raw = stackalloc byte[64];
        RandomNumberGenerator.Fill(raw);
        var token = Convert.ToBase64String(raw);
        var hash = HashRefreshToken(token);
        var expires = _clock.UtcNow.AddDays(_options.RefreshTokenDays);
        return new RefreshTokenResult(token, hash, expires);
    }

    public string HashRefreshToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
