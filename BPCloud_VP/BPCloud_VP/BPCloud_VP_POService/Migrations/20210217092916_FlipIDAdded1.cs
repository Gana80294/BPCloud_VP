using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class FlipIDAdded1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FLIPID",
                table: "BPC_FLIP_Attachment",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BPC_FLIP_Attachment_FLIPID",
                table: "BPC_FLIP_Attachment",
                column: "FLIPID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BPC_FLIP_Attachment_FLIPID",
                table: "BPC_FLIP_Attachment");

            migrationBuilder.AlterColumn<string>(
                name: "FLIPID",
                table: "BPC_FLIP_Attachment",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);
        }
    }
}
