using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadSwap.Data.Migrations
{
    public partial class fixingtypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auther",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Auther",
                table: "Books",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
