using Domain.Entities.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(string userId, string userName, string Role, IEnumerable<Claim>? extra = null);
        string CreateRefreshToken(string userId, string Role);
        ClaimsPrincipal? ValidateRefreshToken(string refreshToken, out SecurityToken? validatedToken);

        /// <summary>SHA-256 hash of a token, hex-encoded. Used to look up the DB record.</summary>
        string HashToken(string token);

        DateTime GetRefreshTokenExpiry();
    }

}
