using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.DTOs;


namespace Application.Queries.Users.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<UserListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserListDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsQueryable();

        // Apply simple filters first
        if (request.selectedRole.HasValue)
        {
            query = query.Where(u => u.Role == request.selectedRole.Value);
        }

        if (request.status != null)
        {
            query = query.Where(u => u.IsActive == request.status);
        }

        if (!string.IsNullOrEmpty(request.selectedCompany))
        {
            query = query.Where(u => u.Company == request.selectedCompany);
        }

        // Search (case-insensitive, null-safe)
        if (!string.IsNullOrWhiteSpace(request.searchQuery))
        {
            var q = request.searchQuery.Trim().ToLower();
            query = query.Where(u =>
                (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(u.LastName) && u.LastName.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(u.IdCode) && u.IdCode.ToLower().Contains(q)) ||
                (!string.IsNullOrEmpty(u.Phone) && u.Phone.Contains(q)) // phone may be numeric or formatted
            );
        }

        var pageNumber = request.pageNumber ?? 1;
        var pageSize = request.pageSize ?? 10;

        // ----- ????? ?? -----
        var totalCount = await query.CountAsync(cancellationToken);

        // ??????? ?? ???? ???? ??? ?? ?????
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        if (pageNumber > totalPages && totalPages > 0)
            pageNumber = totalPages;
        else if (totalPages == 0)
            pageNumber = 1;

        // ----- ????????? ? ????????? -----
        var users = await query
            .OrderByDescending(u => u.CreatedDate)
            .ThenBy(u => u.FirstName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<UserListDto>
        {
            Items = _mapper.Map<List<UserListDto>>(users),
            TotalCount = totalCount
        };


    }
}