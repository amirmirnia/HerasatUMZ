using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitorTypeAndDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Visitors",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "توضیحات تکمیلی ارباب رجوع");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Visitors",
                type: "int",
                nullable: false,
                defaultValue: 1,
                comment: "نوع ارباب رجوع (1 = فرد عادی، 2 = دانشجو)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Visitors");
        }
    }
}
