using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.DTOs.User;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Application.DTOs.User.Auth;

namespace Application.Queries.Users.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;



    public LoginUserQueryHandler(IApplicationDbContext context, IMapper mapper, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        // Find user by email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.IdCode == request.CodeId && u.IsActive, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid IdCode or password");
        }

        // Verify password
        var hashedPassword = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!hashedPassword)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Update last login date
        user.LastLoginDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);



        // Generate JWT token
        var access = _tokenService.CreateAccessToken(user.IdCode, user.FirstName + "" + user.LastName,user.Role.ToString());
        var refresh = _tokenService.CreateRefreshToken(user.IdCode, user.Role.ToString());

        return new LoginResponseDto
        {
            User = _mapper.Map<UserDto>(user),
            Tokenaccess = access,
            Tokenrefresh = refresh,
        };
    }



    private static string GenerateJwtToken(Domain.Entities.Users.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        // Using the same key as configured in Program.cs - in production this should come from IConfiguration
        var key = Encoding.ASCII.GetBytes("ReservationManagementSecretKey12345678901234567890");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}