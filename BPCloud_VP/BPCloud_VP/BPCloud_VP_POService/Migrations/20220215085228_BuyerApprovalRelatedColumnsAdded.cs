using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class BuyerApprovalRelatedColumnsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerApprovalRequired",
                table: "BPC_ASN_H1",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H1",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BuyerApprovedOn",
                table: "BPC_ASN_H1",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerApprovalRequired",
                table: "BPC_ASN_H",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "BuyerApprovedOn",
                table: "BPC_ASN_H",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerApprovedOn",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "IsBuyerApprovalRequired",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "BuyerApprovedOn",
                table: "BPC_ASN_H");

            migrationBuilder.DropColumn(
                name: "IsBuyerApprovalRequired",
                table: "BPC_ASN_H");

            migrationBuilder.DropColumn(
                name: "IsBuyerApproved",
                table: "BPC_ASN_H");
        }
    }
}
