using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class GateEntryDetailsAddedInASN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GateEntryNo",
                table: "BPC_ASN_H1",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GateEntryTime",
                table: "BPC_ASN_H1",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GateEntryNo",
                table: "BPC_ASN_H",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GateEntryTime",
                table: "BPC_ASN_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GateEntryNo",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "GateEntryTime",
                table: "BPC_ASN_H1");

            migrationBuilder.DropColumn(
                name: "GateEntryNo",
                table: "BPC_ASN_H");

            migrationBuilder.DropColumn(
                name: "GateEntryTime",
                table: "BPC_ASN_H");
        }
    }
}
