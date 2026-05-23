using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorRegisteredByName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegisteredByName",
                table: "Visitors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "نام نمایشی کاربری که این ملاقات‌کننده را ثبت کرد");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredByName",
                table: "Visitors");
        }
    }
}
