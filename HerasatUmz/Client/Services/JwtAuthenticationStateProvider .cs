using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net;
using System.Net.Http.Json;
using Application.DTOs.User;

namespace Client.Services
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly AuthenticationState Anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        public JwtAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("HerasatUmz.NoAuth");

                var userDto = await TryFetchMeAsync(client);
                if (userDto == null)
                {
                    var refreshed = await TryRefreshAsync(client);
                    if (refreshed)
                        userDto = await TryFetchMeAsync(client);
                }

                if (userDto == null)
                    return Anonymous;

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, userDto.FullName ?? string.Empty),
                    new(ClaimTypes.NameIdentifier, userDto.IdCode?.ToString() ?? string.Empty),
                    new("Role", userDto.Role.ToString()),
                    new(ClaimTypes.Role, userDto.Role.ToString())
                };

                var identity = new ClaimsIdentity(claims, "cookie");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return Anonymous;
            }
        }

        public void NotifyUserLogout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }

        public void NotifyAuthenticationStateChangedExternal()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private static async Task<UserDto?> TryFetchMeAsync(HttpClient client)
        {
            try
            {
                using var resp = await client.GetAsync("api/auth/me");
                if (resp.StatusCode == HttpStatusCode.Unauthorized || !resp.IsSuccessStatusCode)
                    return null;

                return await resp.Content.ReadFromJsonAsync<UserDto>();
            }
            catch
            {
                return null;
            }
        }

        private static async Task<bool> TryRefreshAsync(HttpClient client)
        {
            try
            {
                using var resp = await client.PostAsync("api/auth/refresh", content: null);
                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
