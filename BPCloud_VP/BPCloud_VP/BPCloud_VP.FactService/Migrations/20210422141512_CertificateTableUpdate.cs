using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class CertificateTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttachmentID",
                table: "BPC_Cert_Support",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttachmentID",
                table: "BPC_Cert",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentID",
                table: "BPC_Cert_Support");

            migrationBuilder.DropColumn(
                name: "AttachmentID",
                table: "BPC_Cert");
        }
    }
}
