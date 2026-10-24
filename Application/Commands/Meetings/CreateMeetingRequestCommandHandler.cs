using Application.Common.Interfaces;
using Application.Common.Exceptions;
using Application.Common.Options;
using Domain.Entities.Meetings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Commands.Meetings;

public class CreateMeetingRequestCommandHandler(
    IApplicationDbContext context,
    IMeetingRequestRegistrationGuard guard,
    IOptions<MeetingApiOptions> options)
    : IRequestHandler<CreateMeetingRequestCommand, int>
{
    public async Task<int> Handle(
        CreateMeetingRequestCommand request,
        CancellationToken cancellationToken)
    {
        var hostNationalCode = request.HostNationalCode.Trim();
        return await guard.ExecuteAsync(hostNationalCode, async () =>
        {
            var minutes = options.Value.RequestIntervalMinutes;
            var cutoff = DateTime.UtcNow.AddMinutes(-minutes);

            if (await context.MeetingRequests.AnyAsync(
                x => x.HostNationalCode == hostNationalCode && x.CreatedDate > cutoff,
                cancellationToken))
            {
                throw new MeetingRequestLimitException(minutes);
            }

            var meeting = new MeetingRequest
            {
                HostFullName = request.HostFullName.Trim(),
                HostNationalCode = hostNationalCode,
                VisitorFullName = request.VisitorFullName.Trim(),
                RequestedDate = request.RequestedDate
            };

            context.MeetingRequests.Add(meeting);
            await context.SaveChangesAsync(cancellationToken);
            return meeting.Id;
        }, cancellationToken);
    }
}
