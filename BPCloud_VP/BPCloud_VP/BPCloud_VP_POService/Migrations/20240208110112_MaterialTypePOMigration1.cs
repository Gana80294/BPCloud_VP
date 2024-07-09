using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class MaterialTypePOMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "BPC_OF_H");

            migrationBuilder.DropColumn(
                name: "MaterialType1",
                table: "BPC_OF_H");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "BPC_OF_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialType1",
                table: "BPC_OF_H",
                nullable: true);
        }
    }
}
