using DDD.BuildingBlocks;

namespace UserApi.Domain.Aggregates;

public class RefreshToken : AggregateRoot
{
    public string Token { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public string CreatedByIp { get; private set; } = string.Empty;
    public string? RevokedByIp { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string token, int expirationDays, string createdByIp)
    {
        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(expirationDays),
            CreatedByIp = createdByIp
        };
    }

    public void Revoke(string? revokedByIp = null, string? replacedByToken = null)
    {
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
        ReplacedByToken = replacedByToken;
        SetUpdated();
    }
}
