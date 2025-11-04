using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class creat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "User first name"),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "User last name"),
                    IdCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false, comment: "User IdCode"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "User email address - unique"),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "User phone number"),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Hashed password"),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "User company name"),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "User job title"),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "User role (Admin, Manager, User)"),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Email verification status"),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Last login date and time"),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Password reset token"),
                    ResetPasswordTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Password reset token expiry date"),
                    EmailVerificationToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Email verification token"),
                    EmailVerificationTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Email verification token expiry date"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_FullName",
                table: "Users",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_User_IsEmailVerified",
                table: "Users",
                column: "IsEmailVerified");

            migrationBuilder.CreateIndex(
                name: "IX_User_Phone",
                table: "Users",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Role",
                table: "Users",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
