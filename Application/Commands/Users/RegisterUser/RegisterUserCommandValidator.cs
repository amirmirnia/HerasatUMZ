using FluentValidation;

namespace Application.Commands.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters");
        RuleFor(x => x.Idcode)
            .NotEmpty()
            .WithMessage("Idcode is required")
            .MaximumLength(15)
            .WithMessage("Idcode cannot exceed 15 characters")
            .MinimumLength(8)
            .WithMessage("Idcode must be at least 8 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(200)
            .WithMessage("Email cannot exceed 200 characters");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^[\+]?[\d\s\-\(\)]{10,20}$")
            .WithMessage("Invalid phone number format")
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required")
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");

        RuleFor(x => x.Company)
            .MaximumLength(200)
            .WithMessage("Company name cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Company));

        RuleFor(x => x.JobTitle)
            .MaximumLength(200)
            .WithMessage("Job title cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.JobTitle));

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Invalid user role");
    }
}