using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ASNTableupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelDuration",
                table: "BPC_Gate_HV");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDuration",
                table: "BPC_ASN_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelDuration",
                table: "BPC_ASN_H");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDuration",
                table: "BPC_Gate_HV",
                nullable: true);
        }
    }
}
