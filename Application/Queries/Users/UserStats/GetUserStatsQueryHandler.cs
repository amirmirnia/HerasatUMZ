using MediatR;
using AutoMapper;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Domain.Enum;

namespace Application.Queries.Users.UserStats;

public class GetUserStatsQueryHandler : IRequestHandler<GetUserStatsQuery, UserStatsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserStatsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserStatsDto> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();


        return new UserStatsDto()
        {
            Active = query.Where(u => u.IsActive == true).Count(),
            Admin= query.Where(u => u.Role == UserRole.Admin).Count(),
            Total = query.Count()
        };

    }
}