namespace Domain.Entities.Users;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>SHA-256 hash of the JWT string. Plain token never lives in the DB.</summary>
    public string TokenHash { get; set; } = string.Empty;

    public string UserIdCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }
    public string? RevokedReason { get; set; }

    /// <summary>If revoked via rotation, points to the token that replaced this one.</summary>
    public Guid? ReplacedByTokenId { get; set; }

    public string? CreatedByIp { get; set; }
    public string? UserAgent { get; set; }
}
