using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class InvoiceTableColumnChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BalanceAmount",
                table: "BPC_INV",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BalanceAmount",
                table: "BPC_INV",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
