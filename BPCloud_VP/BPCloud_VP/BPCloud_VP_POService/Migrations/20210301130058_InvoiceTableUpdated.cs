using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class InvoiceTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AWBNumber",
                table: "BPC_INV",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BalanceAmount",
                table: "BPC_INV",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AWBNumber",
                table: "BPC_INV");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "BPC_INV");
        }
    }
}
