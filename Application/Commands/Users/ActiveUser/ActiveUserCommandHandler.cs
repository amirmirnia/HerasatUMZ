using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Users;

namespace Application.Commands.Users.ActiveUser;

public class ActiveUserCommandHandler : IRequestHandler<ActiveUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ActiveUserCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(ActiveUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }
            user.IsActive = true;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {

            return false;

        }
    }
}