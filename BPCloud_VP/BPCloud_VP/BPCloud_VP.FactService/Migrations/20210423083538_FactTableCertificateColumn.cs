using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class FactTableCertificateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TDS_Cert_No",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TDS_Cert_No",
                table: "BPC_Fact",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TDS_Cert_No",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "TDS_Cert_No",
                table: "BPC_Fact");
        }
    }
}
