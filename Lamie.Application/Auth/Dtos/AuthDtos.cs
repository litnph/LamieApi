using Lamie.Domain.Entities.Auth;

namespace Lamie.Application.Auth.Dtos;

public sealed record AuthUserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = default!;
    public string UserName { get; init; } = default!;
    public string FullName { get; init; } = default!;
    public string? Phone { get; init; }
    public UserRole Role { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginAt { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed record AuthTokensDto
{
    public string AccessToken { get; init; } = default!;
    public DateTime AccessTokenExpiresAt { get; init; }
    public string RefreshToken { get; init; } = default!;
    public DateTime RefreshTokenExpiresAt { get; init; }
}

public sealed record AuthResultDto
{
    public AuthUserDto User { get; init; } = default!;
    public AuthTokensDto Tokens { get; init; } = default!;
}
