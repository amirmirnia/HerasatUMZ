using MediatR;
using Application.DTOs.User;
using Domain.Enum;

namespace Application.Commands.Users.ActiveUser;

public class ActiveUserCommand : IRequest<bool>
{
    public int Id { get; set; }

}