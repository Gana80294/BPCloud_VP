using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class MaterialTypePO1Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterialType1",
                table: "BPC_OF_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialType1",
                table: "BPC_OF_H");
        }
    }
}
