using MediatR;

namespace Application.Commands.Meetings;

public record CreateMeetingRequestCommand(
    string HostFullName,
    string HostNationalCode,
    string VisitorFullName,
    DateOnly RequestedDate) : IRequest<int>;
