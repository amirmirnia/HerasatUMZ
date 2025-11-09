using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Users;
using System.Security.Cryptography;
using System.Text;
using Application.Commands.Users.DeleteUser;
using Domain.Enum;

namespace Application.Commands.Users.RegisterUser;

public class ResetpasswordUserCommandHandler : IRequestHandler<ResetpasswordUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;

    public ResetpasswordUserCommandHandler(
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

    public async Task<bool> Handle(ResetpasswordUserCommand request, CancellationToken cancellationToken)
    {
        // Check if user with same email already exists
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdCode == request.IdCode, cancellationToken);

        if (existingUser.Role == UserRole.Admin)
        {
            throw new InvalidOperationException("please Go to Home");
        }
        if (existingUser == null)
        {
            throw new InvalidOperationException("چنین کاربری ثبت شده است");
        }
        if (request.NewPassword== request.ConfirmNewPassword)
        {
            existingUser.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);

        }
        else
        {
            throw new InvalidOperationException("تکرار پسورد با پسورد جدید همخوانی ندارد");

        }


        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}