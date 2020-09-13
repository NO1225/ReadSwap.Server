using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadSwap.Data.Migrations
{
    public partial class Addingnormalizedbooktitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedTitle",
                table: "Books",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedTitle",
                table: "Books");
        }
    }
}
