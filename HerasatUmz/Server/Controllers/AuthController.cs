using Application.Common.Interfaces;
using Application.DTOs.User.Auth;
using Application.Queries.Users.GetUserById;
using Application.Queries.Users.GetUserByIdcode;
using Application.Queries.Users.LoginUser;
using Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _cfg;


        public AuthController(ITokenService tokenService, IConfiguration cfg)
        {
            _tokenService = tokenService;
            _cfg = cfg;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                LoginUserQuery Login = new LoginUserQuery()
                {
                    CodeId = dto.CodeId,
                    Password = dto.Password

                };
                var resultUser = await Mediator.Send(Login);

                if (resultUser.User == null)
                    return Unauthorized();

                Response.Cookies.Append("access_token", resultUser.Tokenaccess, CookieHelper.CreateCookieOptions(minutes: int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"]!), cfg: _cfg));
                Response.Cookies.Append("refresh_token", resultUser.Tokenrefresh, CookieHelper.CreateCookieOptions(days: int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"]!), isRefresh: true, cfg: _cfg));


                return Ok(resultUser);
            }
            catch (Application.Common.Exceptions.NotFoundException)
            {
                return NotFound(new { Message = $"Payment with ID  not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await Mediator.Send(new GetUserByIdcodeQuery(userId));
            return Ok(user);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
                return Unauthorized();

            var principal = _tokenService.ValidateRefreshToken(refreshToken, out var validatedToken);
            if (principal == null) return Unauthorized();

            var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var userName = "await _userService.GetUserNameByIdAsync(userId);"; // اگر sync، adjust کن
            var Role = principal.FindFirstValue(ClaimTypes.Role);

            // issue new tokens
            var newAccess = _tokenService.CreateAccessToken(userId, userName, Role.ToString());
            var newRefresh = _tokenService.CreateRefreshToken(userId, Role.ToString());

            Response.Cookies.Append("access_token", newAccess, CookieHelper.CreateCookieOptions(minutes: int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"]!), cfg: _cfg));
            Response.Cookies.Append("refresh_token", newRefresh, CookieHelper.CreateCookieOptions(days: int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"]!), isRefresh: true, cfg: _cfg));

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return Ok();
        }
    }

}
