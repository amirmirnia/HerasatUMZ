using Domain.Enum;

namespace Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdCode { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public UserRole Role { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}

public class UserListDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdCode { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Company { get; set; }
    public UserRole Role { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}

public class RegisterUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdCode { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? JobTitle { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public bool IsActive { get; set; }

}

public class UpdateUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string IdCode { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}


public class UserStatsDto
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Admin { get; set; }
}


public class ChangePasswordDto
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordConfirmDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
}

public class VerifyEmailDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}