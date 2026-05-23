using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.DTOs.User.Auth;
using Domain.Entities.Users;

namespace Application.Queries.Users.LoginUser;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUserQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.IdCode == request.CodeId && u.IsActive, cancellationToken);

        if (user == null)
            throw new UnauthorizedAccessException("کد ملی یا رمز عبور نادرست است.");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("کد ملی یا رمز عبور نادرست است.");

        user.LastLoginDate = DateTime.UtcNow;

        var fullName = $"{user.FirstName} {user.LastName}".Trim();
        var role = user.Role.ToString();

        var access = _tokenService.CreateAccessToken(user.IdCode, fullName, role);
        var refresh = _tokenService.CreateRefreshToken(user.IdCode, role);

        _context.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = _tokenService.HashToken(refresh),
            UserIdCode = user.IdCode,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = _tokenService.GetRefreshTokenExpiry(),
            CreatedByIp = request.Ip,
            UserAgent = request.UserAgent
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            User = _mapper.Map<UserDto>(user),
            Tokenaccess = access,
            Tokenrefresh = refresh,
        };
    }
}
