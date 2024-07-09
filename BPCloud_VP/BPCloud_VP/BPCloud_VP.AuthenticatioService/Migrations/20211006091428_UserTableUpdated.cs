using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    public partial class UserTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedOn",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Users");
        }
    }
}
