using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    public partial class UserTableUpdated1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCompanyMaps",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    Company = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyMaps", x => new { x.UserID, x.Company });
                    table.UniqueConstraint("AK_UserCompanyMaps_Company_UserID", x => new { x.Company, x.UserID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCompanyMaps");
        }
    }
}
