using Application.DTOs.Meetings;
using MediatR;

namespace Application.Queries.Meetings;

public record GetMeetingRequestByIdQuery(int Id) : IRequest<MeetingRequestListDto>;
