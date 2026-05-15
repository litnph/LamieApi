namespace Lamie.Infrastructure.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "Lamie";
    public string Audience { get; init; } = "Lamie.Api";
    public string SecretKey { get; init; } = default!;
    public int AccessTokenMinutes { get; init; } = 60;
    public int RefreshTokenDays { get; init; } = 14;
}
