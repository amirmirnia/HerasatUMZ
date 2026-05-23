using MediatR;
using Application.DTOs.User.Auth;

namespace Application.Queries.Users.LoginUser;

public class LoginUserQuery : IRequest<LoginResponseDto>
{
    public string CodeId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    /// <summary>Client IP (for auditing the refresh-token row). Optional.</summary>
    public string? Ip { get; set; }

    /// <summary>Client User-Agent (for auditing). Optional.</summary>
    public string? UserAgent { get; set; }
}
