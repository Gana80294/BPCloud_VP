using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class FactSupportUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "BPC_Fact_Support",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MSME",
                table: "BPC_Fact_Support",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MSME_Att_ID",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MSME_TYPE",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RP",
                table: "BPC_Fact_Support",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RP_Att_ID",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RP_Name",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RP_Type",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reduced_TDS",
                table: "BPC_Fact_Support",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TDS_Att_ID",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TDS_RATE",
                table: "BPC_Fact_Support",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "MSME",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "MSME_Att_ID",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "MSME_TYPE",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "RP",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "RP_Att_ID",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "RP_Name",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "RP_Type",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "Reduced_TDS",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "TDS_Att_ID",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "TDS_RATE",
                table: "BPC_Fact_Support");
        }
    }
}
