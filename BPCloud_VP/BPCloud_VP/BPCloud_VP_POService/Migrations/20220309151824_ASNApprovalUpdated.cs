using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ASNApprovalUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H");

            migrationBuilder.RenameColumn(
                name: "BuyerApprovedOn",
                table: "BPC_ASN_H1",
                newName: "BuyerApprovalOn");

            migrationBuilder.RenameColumn(
                name: "BuyerApprovedOn",
                table: "BPC_ASN_H",
                newName: "BuyerApprovalOn");

            migrationBuilder.AddColumn<string>(
                name: "BuyerApprovalStatus",
                table: "BPC_ASN_H1",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerApprovalStatus",
                table: "BPC_ASN_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerApprovalStatus",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "BuyerApprovalStatus",
                table: "BPC_ASN_H");

            migrationBuilder.RenameColumn(
                name: "BuyerApprovalOn",
                table: "BPC_ASN_H1",
                newName: "BuyerApprovedOn");

            migrationBuilder.RenameColumn(
                name: "BuyerApprovalOn",
                table: "BPC_ASN_H",
                newName: "BuyerApprovedOn");

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H1",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H",
                nullable: false,
                defaultValue: false);
        }
    }
}
