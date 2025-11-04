using MediatR;
using Application.DTOs.User;
using Domain.Enum;

namespace Application.Commands.Users.UpdateUser;

public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string CodeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}