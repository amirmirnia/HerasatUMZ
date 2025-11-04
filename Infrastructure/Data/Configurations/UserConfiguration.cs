using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Users;
using Domain.Enum;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(100)
               .HasComment("User first name");

        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(100)
               .HasComment("User last name");
        builder.Property(u => u.IdCode)
               .IsRequired()
               .HasMaxLength(15)
               .HasComment("User IdCode");

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(200)
               .HasComment("User email address - unique");

        builder.Property(u => u.Phone)
               .IsRequired()
               .HasMaxLength(20)
               .HasComment("User phone number");

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasMaxLength(500)
               .HasComment("Hashed password");

        builder.Property(u => u.Company)
               .HasMaxLength(200)
               .HasComment("User company name");

        builder.Property(u => u.JobTitle)
               .HasMaxLength(200)
               .HasComment("User job title");

        builder.Property(u => u.Role)
               .IsRequired()
               .HasConversion<string>()
               .HasMaxLength(50)
               .HasComment("User role (Admin, Manager, User)");

        builder.Property(u => u.IsEmailVerified)
               .IsRequired()
               .HasDefaultValue(false)
               .HasComment("Email verification status");

        builder.Property(u => u.LastLoginDate)
               .HasComment("Last login date and time");

        builder.Property(u => u.ResetPasswordToken)
               .HasMaxLength(500)
               .HasComment("Password reset token");

        builder.Property(u => u.ResetPasswordTokenExpiry)
               .HasComment("Password reset token expiry date");

        builder.Property(u => u.EmailVerificationToken)
               .HasMaxLength(500)
               .HasComment("Email verification token");

        builder.Property(u => u.EmailVerificationTokenExpiry)
               .HasComment("Email verification token expiry date");

        builder.Property(u => u.CreatedBy)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(u => u.UpdatedBy)
               .HasMaxLength(100);

        // Indexes
        builder.HasIndex(u => u.Email)
               .IsUnique()
               .HasDatabaseName("IX_User_Email");

        builder.HasIndex(u => u.Phone)
               .IsUnique()
               .HasDatabaseName("IX_User_Phone");

        builder.HasIndex(u => u.Role)
               .HasDatabaseName("IX_User_Role");

        builder.HasIndex(u => u.IsEmailVerified)
               .HasDatabaseName("IX_User_IsEmailVerified");

        builder.HasIndex(u => new { u.FirstName, u.LastName })
               .HasDatabaseName("IX_User_FullName");


    }
}