using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ProfitCenterAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfitCentre",
                table: "BPC_FLIP_H",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BPC_ProfitCentre_Master",
                columns: table => new
                {
                    ProfitCentre = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ProfitCentre_Master", x => x.ProfitCentre);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_ProfitCentre_Master");

            migrationBuilder.DropColumn(
                name: "ProfitCentre",
                table: "BPC_FLIP_H");
        }
    }
}
