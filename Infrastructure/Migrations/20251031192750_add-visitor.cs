using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addvisitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Visitors");
        }
    }
}
