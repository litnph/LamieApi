using Lamie.Domain.Entities.Auth;

namespace Lamie.Application.Common.Auth;

public interface IJwtTokenService
{
    AccessTokenResult IssueAccessToken(User user);
    RefreshTokenResult IssueRefreshToken();
    string HashRefreshToken(string token);
}

public sealed record AccessTokenResult(string Token, DateTime ExpiresAt);

public sealed record RefreshTokenResult(string Token, string TokenHash, DateTime ExpiresAt);
