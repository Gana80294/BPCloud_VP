using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.ReportService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_INV",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
                    InvoiceNo = table.Column<string>(maxLength: 16, nullable: false),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    InvoiceAmount = table.Column<double>(nullable: false),
                    PoReference = table.Column<string>(nullable: true),
                    PaidAmount = table.Column<double>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    DateofPayment = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    AttID = table.Column<string>(nullable: true),
                    PODDate = table.Column<DateTime>(nullable: true),
                    PODConfirmedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_INV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.InvoiceNo });
                });

            migrationBuilder.CreateTable(
            name: "BPC_INV_I",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
                    InvoiceNo = table.Column<string>(maxLength: 16, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    InvoiceQty = table.Column<double>(nullable: false),
                    PODQty = table.Column<double>(nullable: false),
                    ReasonCode = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_INV_I", x => new
                    {
                        x.Client,
                        x.Company,
                        x.Type,
                        x.PatnerID,
                        x.FiscalYear,
                        x.InvoiceNo,
                        x.Item
                    });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(maxLength: 4, nullable: false),
                    PaymentDoc = table.Column<string>(maxLength: 10, nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Attachment = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.PaymentDoc });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_DOL",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_DOL", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_FGCPS",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Plant = table.Column<string>(nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    StickQty = table.Column<double>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_FGCPS", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Plant, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_GRR",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    OrderQty = table.Column<double>(nullable: true),
                    ReceivedQty = table.Column<double>(nullable: true),
                    RejectedPPM = table.Column<double>(nullable: true),
                    ReworkQty = table.Column<double>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_GRR", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_IP",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    MaterialChar = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    LowerLimit = table.Column<double>(nullable: true),
                    UpperLimit = table.Column<double>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    ModRule = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_IP", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_OV",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    InputQty = table.Column<double>(nullable: true),
                    AccQty = table.Column<double>(nullable: true),
                    RejQty = table.Column<double>(nullable: true),
                    RejPercentage = table.Column<double>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_OV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_PPM_H",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Period = table.Column<DateTime>(nullable: false),
                    ReceiptQty = table.Column<double>(nullable: true),
                    RejectedQty = table.Column<double>(nullable: true),
                    PPM = table.Column<double>(nullable: true),
                    TotalPPM = table.Column<double>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_PPM_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Period });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_PPM_I",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Period = table.Column<DateTime>(nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    ReceiptQty = table.Column<double>(nullable: true),
                    RejectedQty = table.Column<double>(nullable: true),
                    PPM = table.Column<double>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_PPM_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Period, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_VR",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MaterialText = table.Column<string>(nullable: true),
                    OrderQty = table.Column<double>(nullable: true),
                    ReceivedQty = table.Column<double>(nullable: true),
                    RejectedPPM = table.Column<double>(nullable: true),
                    ReworkQty = table.Column<double>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_VR", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SC_STK",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    PODocument = table.Column<string>(maxLength: 10, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    IssuedQty = table.Column<double>(nullable: false),
                    StockQty = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SC_STK", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.PODocument, x.Item });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_INV");

            migrationBuilder.DropTable(
                name: "BPC_INV_I");

            migrationBuilder.DropTable(
                name: "BPC_PAY");

            migrationBuilder.DropTable(
                name: "BPC_REP_DOL");

            migrationBuilder.DropTable(
                name: "BPC_REP_FGCPS");

            migrationBuilder.DropTable(
                name: "BPC_REP_GRR");

            migrationBuilder.DropTable(
                name: "BPC_REP_IP");

            migrationBuilder.DropTable(
                name: "BPC_REP_OV");

            migrationBuilder.DropTable(
                name: "BPC_REP_PPM_H");

            migrationBuilder.DropTable(
                name: "BPC_REP_PPM_I");

            migrationBuilder.DropTable(
                name: "BPC_REP_VR");

            migrationBuilder.DropTable(
                name: "BPC_SC_STK");
        }
    }
}
