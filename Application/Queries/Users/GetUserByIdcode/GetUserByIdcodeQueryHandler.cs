using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.DTOs.User;
using Domain.Entities.Users;

namespace Application.Queries.Users.GetUserByIdcode;

public class GetUserByIdcodeQueryHandler : IRequestHandler<GetUserByIdcodeQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByIdcodeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdcodeQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.IdCode == request.IdCode && u.IsActive, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.IdCode);
        }

        return _mapper.Map<UserDto>(user);
    }
}