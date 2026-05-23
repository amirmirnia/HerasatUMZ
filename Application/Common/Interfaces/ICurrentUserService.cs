namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? Role { get; }

    /// <summary>True for Admin or Manager roles — used to bypass per-row ownership checks.</summary>
    bool IsPrivileged { get; }
}
