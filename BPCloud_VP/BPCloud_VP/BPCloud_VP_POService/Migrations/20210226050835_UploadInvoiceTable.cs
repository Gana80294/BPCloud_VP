using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class UploadInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OrderedQty",
                table: "BPC_FLIP_I",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "OpenQty",
                table: "BPC_FLIP_I",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "GSTIN",
                table: "BPC_FLIP_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GSTIN",
                table: "BPC_FLIP_H");

            migrationBuilder.AlterColumn<double>(
                name: "OrderedQty",
                table: "BPC_FLIP_I",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "OpenQty",
                table: "BPC_FLIP_I",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
