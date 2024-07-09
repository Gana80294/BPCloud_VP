using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class FactSupportTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GSTNumber",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GSTStatus",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PANNumber",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "BPC_Fact_Support",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GSTNumber",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "GSTStatus",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "PANNumber",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "BPC_Fact_Support");
        }
    }
}
