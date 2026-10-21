using Application.Common.Interfaces;
using Application.DTOs.Meetings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Meetings;

public class GetMeetingRequestsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMeetingRequestsQuery, MeetingRequestPageDto>
{
    public async Task<MeetingRequestPageDto> Handle(
        GetMeetingRequestsQuery request, CancellationToken cancellationToken)
    {
        const int pageSize = 20;
        var query = context.MeetingRequests.AsNoTracking().Where(x => x.IsActive);
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(x => x.HostFullName.Contains(search)
                || x.HostNationalCode.Contains(search) || x.VisitorFullName.Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
        var pageNumber = Math.Clamp(request.PageNumber, 1, totalPages);
        var items = await query.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .Select(x => new MeetingRequestListDto
            {
                Id = x.Id,
                HostFullName = x.HostFullName,
                HostNationalCode = x.HostNationalCode,
                VisitorFullName = x.VisitorFullName,
                RequestedDate = x.RequestedDate,
                CreatedDate = x.CreatedDate,
                Status = x.Status
            }).ToListAsync(cancellationToken);

        return new MeetingRequestPageDto
        {
            Items = items, TotalCount = totalCount, PageNumber = pageNumber, PageSize = pageSize
        };
    }
}
