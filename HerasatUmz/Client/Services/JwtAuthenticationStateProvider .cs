using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Json;
using Application.DTOs.User;
using System.Data;

namespace Client.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        public JwtAuthenticationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // درخواست به API برای گرفتن اطلاعات کاربر
                var userDto = await _httpClient.GetFromJsonAsync<UserDto>("api/auth/me");

                if (userDto == null)
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

                // ساخت Claims از اطلاعات کاربر
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userDto.FullName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, userDto.IdCode.ToString()),
                    new Claim("Role", userDto.Role.ToString()),
                    new Claim(ClaimTypes.Role,userDto.Role.ToString())
                };


                var identity = new ClaimsIdentity(claims, "cookie");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                // اگر کوکی معتبر نبود یا درخواست خطا داد
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }

}
