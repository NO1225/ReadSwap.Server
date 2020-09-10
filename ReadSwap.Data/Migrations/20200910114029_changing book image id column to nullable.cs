using Microsoft.EntityFrameworkCore.Migrations;

namespace ReadSwap.Data.Migrations
{
    public partial class changingbookimageidcolumntonullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_BookImageId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookImageId",
                table: "Books",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookImageId",
                table: "Books",
                column: "BookImageId",
                unique: true,
                filter: "[BookImageId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_BookImageId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookImageId",
                table: "Books",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookImageId",
                table: "Books",
                column: "BookImageId",
                unique: true);
        }
    }
}
