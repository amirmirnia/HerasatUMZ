using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Application.Common.Interfaces;
using Domain.Enum;

namespace Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

    public string? UserId => Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? UserName => Principal?.FindFirst(ClaimTypes.Name)?.Value;

    public string? Email => Principal?.FindFirst(ClaimTypes.Email)?.Value;

    public string? Role => Principal?.FindFirst(ClaimTypes.Role)?.Value
                           ?? Principal?.FindFirst("Role")?.Value;

    public bool IsPrivileged =>
        Role == nameof(UserRole.Admin) || Role == nameof(UserRole.Manager);
}
