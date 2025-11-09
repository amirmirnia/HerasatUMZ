using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Domain.Entities.Users;
using Domain.Enum;

namespace Application.Commands.Users.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user.Role == UserRole.Admin)
        {
            throw new InvalidOperationException("please Go to Home");
        }
        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        // Soft delete - set IsActive to false
        user.IsActive = false;
        
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}