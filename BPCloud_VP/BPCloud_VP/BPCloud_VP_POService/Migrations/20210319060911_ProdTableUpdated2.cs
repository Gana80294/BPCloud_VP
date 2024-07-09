using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ProdTableUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material",
                table: "BPC_Prod_Fav");

            migrationBuilder.RenameColumn(
                name: "Material",
                table: "BPC_Prod",
                newName: "MaterialType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaterialType",
                table: "BPC_Prod",
                newName: "Material");

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "BPC_Prod_Fav",
                nullable: true);
        }
    }
}
