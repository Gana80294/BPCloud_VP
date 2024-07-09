using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class GateEntryCreatedByAddedInASN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GateEntryCreatedBy",
                table: "BPC_ASN_H1",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GateEntryCreatedBy",
                table: "BPC_ASN_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GateEntryCreatedBy",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "GateEntryCreatedBy",
                table: "BPC_ASN_H");
        }
    }
}
