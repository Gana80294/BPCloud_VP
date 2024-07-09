using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class FreightAddedInASNItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFreightAvailable",
                table: "BPC_ASN_I1",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FreightAmount",
                table: "BPC_ASN_I1",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFreightAvailable",
                table: "BPC_ASN_I",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "FreightAmount",
                table: "BPC_ASN_I",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreightAmount",
                table: "BPC_ASN_I1");

            migrationBuilder.DropColumn(
                name: "IsFreightAvailable",
                table: "BPC_ASN_I1");

            migrationBuilder.DropColumn(
                name: "FreightAmount",
                table: "BPC_ASN_I");

            migrationBuilder.DropColumn(
                name: "IsFreightAvailable",
                table: "BPC_ASN_I");
        }
    }
}
