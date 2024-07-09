using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    public partial class PasswordPolicyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiringOn",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pass1",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pass2",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pass3",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiringOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pass1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pass2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pass3",
                table: "Users");
        }
    }
}
