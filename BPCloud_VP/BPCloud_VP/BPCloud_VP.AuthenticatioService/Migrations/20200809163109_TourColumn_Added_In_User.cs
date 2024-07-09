using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.AuthenticatioService.Migrations
{
    public partial class TourColumn_Added_In_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TourStatus",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourStatus",
                table: "Users");
        }
    }
}
