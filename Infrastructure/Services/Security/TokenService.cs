using Application.Common.Interfaces;
using Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _cfg;
        private readonly JwtSecurityTokenHandler _handler = new();

        public TokenService(IConfiguration cfg) => _cfg = cfg;

        private SymmetricSecurityKey GetKey() =>
            new(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));

        public string CreateAccessToken(string userId, string userName, string Role, IEnumerable<Claim>? extra = null)
        {
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenExpirationMinutes"]!));
            var creds = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName ?? string.Empty),
            new Claim("Role", Role.ToString()),
            new Claim(ClaimTypes.Role, Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("typ", "access")
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

        public string CreateRefreshToken(string userId, string Role)
        {
            var expires = DateTime.UtcNow.AddDays(int.Parse(_cfg["Jwt:RefreshTokenExpirationDays"]!));
            var creds = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, Role.ToString()),
             new Claim("Role", Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("typ", "refresh")
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
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
                var principal = _handler.ValidateToken(refreshToken, prms, out validatedToken);
                // ensure typ == refresh
                var jwt = validatedToken as JwtSecurityToken;
                if (jwt == null || jwt.Claims.FirstOrDefault(c => c.Type == "typ")?.Value != "refresh")
                    return null;
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
