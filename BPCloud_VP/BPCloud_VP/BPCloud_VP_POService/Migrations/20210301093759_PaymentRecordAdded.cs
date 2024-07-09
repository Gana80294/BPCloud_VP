using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class PaymentRecordAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_PAY_RECORD",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(maxLength: 12, nullable: false),
                    DocumentNumber = table.Column<string>(maxLength: 10, nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: false),
                    PayRecordNo = table.Column<string>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    Medium = table.Column<string>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    RefNumber = table.Column<string>(nullable: true),
                    PayDoc = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_RECORD", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.DocumentNumber, x.InvoiceNumber, x.PayRecordNo });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_PAY_RECORD");
        }
    }
}
