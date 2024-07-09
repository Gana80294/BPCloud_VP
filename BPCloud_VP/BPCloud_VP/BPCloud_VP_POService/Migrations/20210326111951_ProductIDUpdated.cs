using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ProductIDUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductID",
                table: "BPC_Prod_Fav",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "ProductID",
                table: "BPC_Prod",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 12);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductID",
                table: "BPC_Prod_Fav",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 18);

            migrationBuilder.AlterColumn<string>(
                name: "ProductID",
                table: "BPC_Prod",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 18);
        }
    }
}
