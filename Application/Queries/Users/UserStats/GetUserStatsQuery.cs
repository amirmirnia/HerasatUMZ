using MediatR;
using Application.DTOs.User;
using Domain.Enum;
using Application.DTOs;

namespace Application.Queries.Users.UserStats;

public class GetUserStatsQuery : IRequest<UserStatsDto>
{

}
