using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class GateEntryDateUpdatedInASN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GateEntryTime",
                table: "BPC_ASN_H1",
                newName: "GateEntryDate");

            migrationBuilder.RenameColumn(
                name: "GateEntryTime",
                table: "BPC_ASN_H",
                newName: "GateEntryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GateEntryDate",
                table: "BPC_ASN_H1",
                newName: "GateEntryTime");

            migrationBuilder.RenameColumn(
                name: "GateEntryDate",
                table: "BPC_ASN_H",
                newName: "GateEntryTime");
        }
    }
}
