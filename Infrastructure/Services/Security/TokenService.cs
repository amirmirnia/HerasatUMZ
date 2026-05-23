using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _cfg;
        private readonly JwtSecurityTokenHandler _handler = new();

        public TokenService(IConfiguration cfg) => _cfg = cfg;

        private SymmetricSecurityKey GetKey() =>
            new(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));

        public string CreateAccessToken(string userId, string userName, string role, IEnumerable<Claim>? extra = null)
        {
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"]!));
            var creds = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Name, userName ?? string.Empty),
                new(ClaimTypes.Role, role ?? string.Empty),
                new("Role", role ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("typ", "access")
            };
            if (extra != null) claims.AddRange(extra);

            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return _handler.WriteToken(token);
        }

        public string CreateRefreshToken(string userId, string role)
        {
            var expires = DateTime.UtcNow.AddDays(int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"]!));
            var creds = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Role, role ?? string.Empty),
                new("Role", role ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("typ", "refresh")
            };
            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return _handler.WriteToken(token);
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }

        public DateTime GetRefreshTokenExpiry() =>
            DateTime.UtcNow.AddDays(int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"]!));

        public ClaimsPrincipal? ValidateRefreshToken(string refreshToken, out SecurityToken? validatedToken)
        {
            validatedToken = null;
            try
            {
                var prms = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _cfg["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _cfg["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = GetKey(),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role
                };
                var principal = _handler.ValidateToken(refreshToken, prms, out validatedToken);

                if (validatedToken is not JwtSecurityToken jwt ||
                    jwt.Claims.FirstOrDefault(c => c.Type == "typ")?.Value != "refresh")
                {
                    validatedToken = null;
                    return null;
                }
                return principal;
            }
            catch
            {
                validatedToken = null;
                return null;
            }
        }
    }
}
