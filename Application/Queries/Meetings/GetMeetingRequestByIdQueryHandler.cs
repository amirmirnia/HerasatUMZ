using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Meetings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Meetings;

public class GetMeetingRequestByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMeetingRequestByIdQuery, MeetingRequestListDto>
{
    public async Task<MeetingRequestListDto> Handle(
        GetMeetingRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.MeetingRequests.AsNoTracking()
            .Where(x => x.Id == request.Id && x.IsActive)
            .Select(x => new MeetingRequestListDto
            {
                Id = x.Id,
                HostFullName = x.HostFullName,
                HostNationalCode = x.HostNationalCode,
                VisitorFullName = x.VisitorFullName,
                RequestedDate = x.RequestedDate,
                CreatedDate = x.CreatedDate,
                Status = x.Status
            }).FirstOrDefaultAsync(cancellationToken);

        return result ?? throw new NotFoundException("درخواست ملاقات یافت نشد.");
    }
}
