using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class OFAndCompanyMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "POCreator",
                table: "BPC_OF_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BPC_OF_H",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BPC_Company_Master",
                columns: table => new
                {
                    Company = table.Column<string>(nullable: false),
                    CompanyName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Company_Master", x => x.Company);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_Company_Master");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BPC_OF_H");

            migrationBuilder.DropColumn(
                name: "POCreator",
                table: "BPC_OF_H");
        }
    }
}
