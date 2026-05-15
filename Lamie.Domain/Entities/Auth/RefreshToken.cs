namespace Lamie.Domain.Entities.Auth;

public class RefreshToken : Entity
{
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = default!;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }
    public string? CreatedByIp { get; private set; }
    public string? RevokedByIp { get; private set; }

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;

    private RefreshToken() { }

    public RefreshToken(Guid userId, string tokenHash, DateTime expiresAt, string? createdByIp = null)
    {
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp;
    }

    public void Revoke(DateTime utcNow, string? revokedByIp = null, Guid? replacedBy = null)
    {
        if (RevokedAt.HasValue) return;

        RevokedAt = utcNow;
        RevokedByIp = revokedByIp;
        ReplacedByTokenId = replacedBy;
    }
}
