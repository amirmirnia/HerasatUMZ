using Application.DTOs.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Client.Services
{
    public class UserContextService
    {
        private readonly AuthenticationStateProvider _authProvider;
        private readonly HttpClient _httpClient;
        private ClaimsPrincipal? _user;
        private UserDto? _userDetails;

        public UserContextService(AuthenticationStateProvider authProvider, HttpClient httpClient)
        {
            _authProvider = authProvider;
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            _user = authState.User;

            if (_user?.Identity?.IsAuthenticated == true)
            {
                await LoadUserDetailsFromApi();
            }

        }

        private async Task LoadUserDetailsFromApi()
        {
            try
            {

                _userDetails = await _httpClient.GetFromJsonAsync<UserDto>($"api/Users/me");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user details: {ex.Message}");
            }
        }

        // اطلاعات از Claims
        public string? Name => _user?.Identity?.Name;
        public string? Role => _user?.FindFirst(ClaimTypes.Role)?.Value;
        public string? Userid => _user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        public string? NationalCode => _user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // اطلاعات کامل از API
        public UserDto? UserDetails => _userDetails;
    }
}
