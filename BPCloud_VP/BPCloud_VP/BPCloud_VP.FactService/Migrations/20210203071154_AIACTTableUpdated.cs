using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class AIACTTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActType",
                table: "BPC_AI_ACT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "BPC_AI_ACT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActType",
                table: "BPC_AI_ACT");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "BPC_AI_ACT");
        }
    }
}
