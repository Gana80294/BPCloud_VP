using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ReturnBatchAndSerialOrderQtyRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderQty",
                table: "BPC_Ret_I_Serial");

            migrationBuilder.DropColumn(
                name: "OrderQty",
                table: "BPC_Ret_I_Batch");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OrderQty",
                table: "BPC_Ret_I_Serial",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrderQty",
                table: "BPC_Ret_I_Batch",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
