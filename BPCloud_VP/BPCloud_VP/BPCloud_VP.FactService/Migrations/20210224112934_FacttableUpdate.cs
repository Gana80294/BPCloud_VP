using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class FacttableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "BPC_Fact_Bank_Support",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "BPC_Fact_Bank_Support",
                newName: "AccountNo");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "BPC_Fact_Bank",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "BPC_Fact_Bank",
                newName: "AccountNo");

            migrationBuilder.AddColumn<string>(
                name: "AccountGroup",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrg",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeofIndustry",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                table: "BPC_Fact_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "BPC_Fact_CP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "BPC_Fact_CP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "BPC_Fact_CP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BPC_Fact_CP",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "BPC_Fact_Bank_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "BPC_Fact_Bank_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "BPC_Fact_Bank_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IFSC",
                table: "BPC_Fact_Bank_Support",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "BPC_Fact_Bank",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "BPC_Fact_Bank",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "BPC_Fact_Bank",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IFSC",
                table: "BPC_Fact_Bank",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountGroup",
                table: "BPC_Fact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "BPC_Fact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BPC_Fact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrg",
                table: "BPC_Fact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "BPC_Fact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeofIndustry",
                table: "BPC_Fact",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                table: "BPC_Fact",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountGroup",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "PurchaseOrg",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "TypeofIndustry",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "VendorCode",
                table: "BPC_Fact_Support");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "BPC_Fact_CP");

            migrationBuilder.DropColumn(
                name: "Item",
                table: "BPC_Fact_CP");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "BPC_Fact_CP");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BPC_Fact_CP");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "BPC_Fact_Bank_Support");

            migrationBuilder.DropColumn(
                name: "City",
                table: "BPC_Fact_Bank_Support");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "BPC_Fact_Bank_Support");

            migrationBuilder.DropColumn(
                name: "IFSC",
                table: "BPC_Fact_Bank_Support");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "BPC_Fact_Bank");

            migrationBuilder.DropColumn(
                name: "City",
                table: "BPC_Fact_Bank");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "BPC_Fact_Bank");

            migrationBuilder.DropColumn(
                name: "IFSC",
                table: "BPC_Fact_Bank");

            migrationBuilder.DropColumn(
                name: "AccountGroup",
                table: "BPC_Fact");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "BPC_Fact");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BPC_Fact");

            migrationBuilder.DropColumn(
                name: "PurchaseOrg",
                table: "BPC_Fact");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "BPC_Fact");

            migrationBuilder.DropColumn(
                name: "TypeofIndustry",
                table: "BPC_Fact");

            migrationBuilder.DropColumn(
                name: "VendorCode",
                table: "BPC_Fact");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BPC_Fact_Bank_Support",
                newName: "AccountName");

            migrationBuilder.RenameColumn(
                name: "AccountNo",
                table: "BPC_Fact_Bank_Support",
                newName: "AccountNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BPC_Fact_Bank",
                newName: "AccountName");

            migrationBuilder.RenameColumn(
                name: "AccountNo",
                table: "BPC_Fact_Bank",
                newName: "AccountNumber");
        }
    }
}
