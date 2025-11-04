using MediatR;
using Application.DTOs.User;

namespace Application.Queries.Users.GetUserById;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}