using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ProdTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "BPC_Prod",
                newName: "MaterialText");

            migrationBuilder.AddColumn<string>(
                name: "BasePrice",
                table: "BPC_Prod",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "BPC_Prod",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialGroup",
                table: "BPC_Prod",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BPC_Prod_Fav",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ProductID = table.Column<string>(maxLength: 12, nullable: false),
                    Material = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Prod_Fav", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ProductID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_Prod_Fav");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "BPC_Prod");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "BPC_Prod");

            migrationBuilder.DropColumn(
                name: "MaterialGroup",
                table: "BPC_Prod");

            migrationBuilder.RenameColumn(
                name: "MaterialText",
                table: "BPC_Prod",
                newName: "Text");
        }
    }
}
