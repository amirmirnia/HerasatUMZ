using MediatR;

namespace Application.Commands.Users.DeleteUser;

public class ResetpasswordUserCommand : IRequest<bool>
{
    public string IdCode { get; set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}