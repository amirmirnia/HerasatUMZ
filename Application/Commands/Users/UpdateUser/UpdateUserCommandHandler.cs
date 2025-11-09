using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Enum;

namespace Application.Commands.Users.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user.Role==UserRole.Admin)
        {
            throw new InvalidOperationException("please Go to Home");
        }
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        // Check if email is being changed and if new email already exists
        if (user.Email != request.Email)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Id != request.Id, cancellationToken);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            
            // If email is changed, mark as unverified
            user.IsEmailVerified = false;
        }

        // Check if phone is being changed and if new phone already exists
        if (user.Phone != request.Phone)
        {
            var existingUserByPhone = await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == request.Phone && u.Id != request.Id, cancellationToken);

            if (existingUserByPhone != null)
            {
                throw new InvalidOperationException("User with this phone number already exists");
            }
        }

        // Update user properties
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Phone = request.Phone;
        user.Company = request.Company;
        user.JobTitle = request.JobTitle;
        user.Role = request.Role;
        user.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}