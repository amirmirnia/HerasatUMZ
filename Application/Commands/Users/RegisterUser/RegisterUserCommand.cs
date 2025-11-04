using MediatR;
using Application.DTOs.User;
using Domain.Enum;

namespace Application.Commands.Users.RegisterUser;

public class RegisterUserCommand : IRequest<UserDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Idcode { get; set; } 
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
}