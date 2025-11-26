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

            migrationBuilder.CreateTable(
                name: "visitLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codeid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Page = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام و نام خانوادگی ملاقات‌کننده"),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "کد ملی ملاقات‌کننده"),
                    HostName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "نام ملاقات‌شونده در دانشگاه"),
                    RegisterDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "تاریخ و زمان ورود به دانشگاه"),
                    GuidCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "کد یکتا برای پیگیری ملاقات"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true, comment: "شماره همراه ملاقات‌کننده"),
                    PhotoPath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "مسیر ذخیره تصویر ملاقات‌کننده"),
                    IsInside = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "وضعیت حضور ملاقات‌کننده در داخل دانشگاه (true = داخل)"),
                    ExitDateTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "تاریخ و زمان خروج از دانشگاه"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "کاربری که این رکورد را ثبت کرده است"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "کاربری که آخرین تغییر را انجام داده است"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatePart1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlateLetter = table.Column<int>(type: "int", nullable: true),
                    PlatePart3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlatePart4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehiclePhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitorId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VisitorId",
                table: "Vehicles",
                column: "VisitorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitor_GuidCode",
                table: "Visitors",
                column: "GuidCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitor_IsInside",
                table: "Visitors",
                column: "IsInside");

            migrationBuilder.CreateIndex(
                name: "IX_Visitor_NationalCode",
                table: "Visitors",
                column: "NationalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Visitor_RegisterDateTime",
                table: "Visitors",
                column: "RegisterDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "visitLogs");

            migrationBuilder.DropTable(
                name: "Visitors");
        }
    }
}
