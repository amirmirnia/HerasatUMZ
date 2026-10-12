using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RemoveVisitorNationalCodeLengthLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Visitor_NationalCode", table: "Visitors");

            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                table: "Visitors",
                type: "nvarchar(max)",
                nullable: false,
                comment: "کد ملی ملاقات‌کننده",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldComment: "کد ملی ملاقات‌کننده");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                table: "Visitors",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                comment: "کد ملی ملاقات‌کننده",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "کد ملی ملاقات‌کننده");

            migrationBuilder.CreateIndex(name: "IX_Visitor_NationalCode", table: "Visitors", column: "NationalCode");
        }
    }
}