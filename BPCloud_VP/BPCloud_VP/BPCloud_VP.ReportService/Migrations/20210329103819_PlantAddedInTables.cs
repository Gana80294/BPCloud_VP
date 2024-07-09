using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.ReportService.Migrations
{
    public partial class PlantAddedInTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_SC_STK",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_PAY",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_INV",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_SC_STK");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_PAY");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_INV");
        }
    }
}
