using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    public partial class UserSupportMasterMapAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSupportMasterMaps",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    ReasonCode = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSupportMasterMaps", x => new { x.UserID, x.ReasonCode });
                    table.UniqueConstraint("AK_UserSupportMasterMaps_ReasonCode_UserID", x => new { x.ReasonCode, x.UserID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSupportMasterMaps");
        }
    }
}
