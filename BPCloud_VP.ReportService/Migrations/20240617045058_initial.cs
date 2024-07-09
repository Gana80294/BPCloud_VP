using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BPCloud_VP.ReportService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_INV",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    InvoiceNo = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    ASN = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ASNDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PostingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: false),
                    PoReference = table.Column<string>(type: "text", nullable: true),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    DateofPayment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    AttID = table.Column<string>(type: "text", nullable: true),
                    PODDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PODConfirmedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_INV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.InvoiceNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_INV_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    InvoiceNo = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    InvoiceQty = table.Column<double>(type: "double precision", nullable: false),
                    PODQty = table.Column<double>(type: "double precision", nullable: false),
                    ReasonCode = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_INV_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.InvoiceNo, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    PaymentDoc = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.PaymentDoc });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_DOL",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Material = table.Column<string>(type: "text", nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_DOL", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_FGCPS",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Plant = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Material = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    StickQty = table.Column<double>(type: "double precision", nullable: true),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    Batch = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_FGCPS", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Plant, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_GRR",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Material = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    OrderQty = table.Column<double>(type: "double precision", nullable: true),
                    ReceivedQty = table.Column<double>(type: "double precision", nullable: true),
                    RejectedPPM = table.Column<double>(type: "double precision", nullable: true),
                    ReworkQty = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_GRR", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_IP",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Material = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    MaterialChar = table.Column<string>(type: "text", nullable: true),
                    Desc = table.Column<string>(type: "text", nullable: true),
                    LowerLimit = table.Column<double>(type: "double precision", nullable: true),
                    UpperLimit = table.Column<double>(type: "double precision", nullable: true),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    Method = table.Column<string>(type: "text", nullable: true),
                    ModRule = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_IP", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_OV",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Material = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    InputQty = table.Column<double>(type: "double precision", nullable: true),
                    AccQty = table.Column<double>(type: "double precision", nullable: true),
                    RejQty = table.Column<double>(type: "double precision", nullable: true),
                    RejPercentage = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_OV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_PPM_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Period = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiptQty = table.Column<double>(type: "double precision", nullable: true),
                    RejectedQty = table.Column<double>(type: "double precision", nullable: true),
                    PPM = table.Column<double>(type: "double precision", nullable: true),
                    TotalPPM = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_PPM_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Period });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_PPM_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Period = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Material = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    ReceiptQty = table.Column<double>(type: "double precision", nullable: true),
                    RejectedQty = table.Column<double>(type: "double precision", nullable: true),
                    PPM = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_PPM_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Period, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_REP_VR",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Material = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    OrderQty = table.Column<double>(type: "double precision", nullable: true),
                    ReceivedQty = table.Column<double>(type: "double precision", nullable: true),
                    RejectedPPM = table.Column<double>(type: "double precision", nullable: true),
                    ReworkQty = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_REP_VR", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.Material });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SC_STK",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    PODocument = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    IssuedQty = table.Column<double>(type: "double precision", nullable: true),
                    StockQty = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SC_STK", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.PODocument, x.Item });
                });
        }

        /// <inheritdoc />
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
