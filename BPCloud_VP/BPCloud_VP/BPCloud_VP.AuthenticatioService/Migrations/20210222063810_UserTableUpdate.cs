using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    public partial class UserTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsLockDuration",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastChangedPassword",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pass4",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pass5",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsLockDuration",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastChangedPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pass4",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pass5",
                table: "Users");
        }
    }
}
