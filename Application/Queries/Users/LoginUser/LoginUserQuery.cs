using MediatR;
using Application.DTOs.User;
using Application.DTOs.User.Auth;

namespace Application.Queries.Users.LoginUser;

public class LoginUserQuery : IRequest<LoginResponseDto>
{
    public string CodeId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}