using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class PlantAddedInTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_POD_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_PAY_AS",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_OF_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_OF_GRGI",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_INV",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_FLIP_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_ASN_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BP_BC_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_POD_H");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_PAY_AS");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_OF_H");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_OF_GRGI");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_INV");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_FLIP_H");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_ASN_H");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BP_BC_H");
        }
    }
}
