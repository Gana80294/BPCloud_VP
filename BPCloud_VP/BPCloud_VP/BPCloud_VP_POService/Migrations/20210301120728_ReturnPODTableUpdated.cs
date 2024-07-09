using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ReturnPODTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "BPC_Ret_I",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AWBNumber",
                table: "BPC_Ret_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreditNote",
                table: "BPC_Ret_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Transporter",
                table: "BPC_Ret_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruckNumber",
                table: "BPC_Ret_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Driver",
                table: "BPC_POD_H",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverContactNo",
                table: "BPC_POD_H",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "BPC_Ret_I");

            migrationBuilder.DropColumn(
                name: "AWBNumber",
                table: "BPC_Ret_H");

            migrationBuilder.DropColumn(
                name: "CreditNote",
                table: "BPC_Ret_H");

            migrationBuilder.DropColumn(
                name: "Transporter",
                table: "BPC_Ret_H");

            migrationBuilder.DropColumn(
                name: "TruckNumber",
                table: "BPC_Ret_H");

            migrationBuilder.DropColumn(
                name: "Driver",
                table: "BPC_POD_H");

            migrationBuilder.DropColumn(
                name: "DriverContactNo",
                table: "BPC_POD_H");
        }
    }
}
