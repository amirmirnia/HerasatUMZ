using MediatR;

namespace Application.Commands.Users.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}