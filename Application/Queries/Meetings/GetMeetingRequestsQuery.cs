using Application.DTOs.Meetings;
using MediatR;

namespace Application.Queries.Meetings;

public record GetMeetingRequestsQuery(string? Search, int PageNumber = 1)
    : IRequest<MeetingRequestPageDto>;
