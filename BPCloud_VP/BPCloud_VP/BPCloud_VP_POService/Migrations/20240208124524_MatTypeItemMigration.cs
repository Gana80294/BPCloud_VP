using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class MatTypeItemMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "BPC_OF_H");

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "BPC_OF_I",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "BPC_OF_I");

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "BPC_OF_H",
                nullable: true);
        }
    }
}
