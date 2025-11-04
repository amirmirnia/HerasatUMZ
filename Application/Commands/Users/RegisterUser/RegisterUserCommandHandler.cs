using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Users;
using System.Security.Cryptography;
using System.Text;

namespace Application.Commands.Users.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IApplicationDbContext context, 
        IMapper mapper,
        ICurrentUserService currentUserService,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user with same email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("چنین ایمیلی ثبت شده است");
        }

        // Check if user with same phone already exists
        var existingUserByPhone = await _context.Users
            .FirstOrDefaultAsync(u => u.Phone == request.Phone, cancellationToken);

        if (existingUserByPhone != null)
        {
            throw new InvalidOperationException("چنین شماره تماسی ثبت شده است");
        }
        var existingUserByIdCode = await _context.Users
            .FirstOrDefaultAsync(u => u.IdCode == request.Idcode, cancellationToken);

        if (existingUserByIdCode != null)
        {
            throw new InvalidOperationException("چنین کد ملی وجود دارد");
        }
        
        // Create new user
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdCode=request.Idcode,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Company = request.Company,
            JobTitle = request.JobTitle,
            Role = request.Role,
            IsEmailVerified = true,
            EmailVerificationToken = GenerateToken(),
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddDays(1)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }

 

    private static string GenerateToken()
    {
        var tokenBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }
}