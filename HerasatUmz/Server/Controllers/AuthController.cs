using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User.Auth;
using Application.Queries.Users.GetUserByIdcode;
using Application.Queries.Users.LoginUser;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Helpers;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IApplicationDbContext _db;
        private readonly IConfiguration _cfg;

        public AuthController(
            ITokenService tokenService,
            IApplicationDbContext db,
            IConfiguration cfg)
        {
            _tokenService = tokenService;
            _db = db;
            _cfg = cfg;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await Mediator.Send(new LoginUserQuery
            {
                CodeId = dto.CodeId,
                Password = dto.Password,
                Ip = GetClientIp(),
                UserAgent = Request.Headers.UserAgent.ToString()
            });

            SetAuthCookies(result.Tokenaccess, result.Tokenrefresh);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("ابتدا وارد سیستم شوید.");

            var user = await Mediator.Send(new GetUserByIdcodeQuery(userId));
            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var refreshJwt) ||
                string.IsNullOrWhiteSpace(refreshJwt))
            {
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "refresh_missing",
                    Message = "نشست شما منقضی شده است. لطفاً دوباره وارد شوید."
                });
            }

            var principal = _tokenService.ValidateRefreshToken(refreshJwt, out _);
            if (principal == null)
            {
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "refresh_invalid",
                    Message = "نشست شما نامعتبر است. لطفاً دوباره وارد شوید."
                });
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "refresh_invalid",
                    Message = "نشست شما نامعتبر است. لطفاً دوباره وارد شوید."
                });
            }

            var hash = _tokenService.HashToken(refreshJwt);
            var stored = await _db.RefreshTokens
                .FirstOrDefaultAsync(t => t.TokenHash == hash, cancellationToken);

            if (stored == null)
            {
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "refresh_unknown",
                    Message = "نشست شما نامعتبر است. لطفاً دوباره وارد شوید."
                });
            }

            if (stored.RevokedAt != null)
            {
                await RevokeAllActiveTokensForUserAsync(stored.UserIdCode, "reuse_detected", cancellationToken);
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "refresh_reuse",
                    Message = "ناهنجاری امنیتی شناسایی شد. تمام نشست‌های شما بسته شد."
                });
            }

            if (stored.ExpiresAt <= DateTime.UtcNow)
            {
                stored.RevokedAt = DateTime.UtcNow;
                stored.RevokedReason = "expired";
                await _db.SaveChangesAsync(cancellationToken);
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "refresh_expired",
                    Message = "نشست شما منقضی شده است. لطفاً دوباره وارد شوید."
                });
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.IdCode == userId, cancellationToken);

            if (user == null || !user.IsActive)
            {
                stored.RevokedAt = DateTime.UtcNow;
                stored.RevokedReason = "user_inactive";
                await _db.SaveChangesAsync(cancellationToken);
                ClearAuthCookies();
                return Unauthorized(new Application.Common.Errors.ApiError
                {
                    Code = "user_inactive",
                    Message = "حساب کاربری شما غیرفعال شده است."
                });
            }

            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            var role = user.Role.ToString();

            var newAccess = _tokenService.CreateAccessToken(user.IdCode, fullName, role);
            var newRefresh = _tokenService.CreateRefreshToken(user.IdCode, role);

            var newRow = new RefreshToken
            {
                TokenHash = _tokenService.HashToken(newRefresh),
                UserIdCode = user.IdCode,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = _tokenService.GetRefreshTokenExpiry(),
                CreatedByIp = GetClientIp(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            stored.RevokedAt = DateTime.UtcNow;
            stored.RevokedReason = "rotation";
            stored.ReplacedByTokenId = newRow.Id;

            _db.RefreshTokens.Add(newRow);
            await _db.SaveChangesAsync(cancellationToken);

            SetAuthCookies(newAccess, newRefresh);
            return Ok(new { Message = "نشست با موفقیت تمدید شد." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var refreshJwt) &&
                !string.IsNullOrWhiteSpace(refreshJwt))
            {
                var hash = _tokenService.HashToken(refreshJwt);
                var stored = await _db.RefreshTokens
                    .FirstOrDefaultAsync(t => t.TokenHash == hash, cancellationToken);

                if (stored is { RevokedAt: null })
                {
                    stored.RevokedAt = DateTime.UtcNow;
                    stored.RevokedReason = "logout";
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }

            ClearAuthCookies();
            return Ok();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("ابتدا وارد سیستم شوید.");

            await RevokeAllActiveTokensForUserAsync(userId, "logout_all", cancellationToken);
            ClearAuthCookies();
            return Ok();
        }

        private async Task RevokeAllActiveTokensForUserAsync(string userIdCode, string reason, CancellationToken ct)
        {
            var active = await _db.RefreshTokens
                .Where(t => t.UserIdCode == userIdCode && t.RevokedAt == null)
                .ToListAsync(ct);

            var now = DateTime.UtcNow;
            foreach (var t in active)
            {
                t.RevokedAt = now;
                t.RevokedReason = reason;
            }
            if (active.Count > 0)
                await _db.SaveChangesAsync(ct);
        }

        private void SetAuthCookies(string accessToken, string refreshToken)
        {
            var accessMinutes = int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"]!);
            var refreshDays = int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"]!);

            Response.Cookies.Append("access_token", accessToken,
                CookieHelper.CreateCookieOptions(minutes: accessMinutes, cfg: _cfg));

            Response.Cookies.Append("refresh_token", refreshToken,
                CookieHelper.CreateCookieOptions(days: refreshDays, isRefresh: true, cfg: _cfg));
        }

        private void ClearAuthCookies()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token", CookieHelper.DeleteOptions(isRefresh: true));
        }

        private string? GetClientIp()
        {
            var fwd = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(fwd))
                return fwd.Split(',').First().Trim();
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
