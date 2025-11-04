using MediatR;
using Application.DTOs.User;

namespace Application.Queries.Users.GetUserByIdcode;

public class GetUserByIdcodeQuery : IRequest<UserDto>
{
    public string IdCode { get; set; }
    public GetUserByIdcodeQuery(string idCode)
    {
        IdCode = idCode;
    }
}