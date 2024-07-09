using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ToleranceLimitAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<double>(
                name: "ToleranceUpperLimit",
                table: "BPC_OF_I",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceLowerLimit",
                table: "BPC_OF_I",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceUpperLimit",
                table: "BPC_ASN_I1",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceLowerLimit",
                table: "BPC_ASN_I1",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceUpperLimit",
                table: "BPC_ASN_I",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ToleranceLowerLimit",
                table: "BPC_ASN_I",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToleranceLowerLimit",
                table: "BPC_OF_I");

            migrationBuilder.DropColumn(
                name: "ToleranceUpperLimit",
                table: "BPC_OF_I");

            migrationBuilder.DropColumn(
                name: "ToleranceLowerLimit",
                table: "BPC_ASN_I1");

            migrationBuilder.DropColumn(
                name: "ToleranceUpperLimit",
                table: "BPC_ASN_I1");

            migrationBuilder.DropColumn(
                name: "ToleranceLowerLimit",
                table: "BPC_ASN_I");

            migrationBuilder.DropColumn(
                name: "ToleranceUpperLimit",
                table: "BPC_ASN_I");
        }
    }
}
