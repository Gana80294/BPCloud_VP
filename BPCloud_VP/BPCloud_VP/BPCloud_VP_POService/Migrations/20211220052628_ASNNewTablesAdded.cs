using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ASNNewTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_ASN_H1",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
                    ASNDate = table.Column<DateTime>(nullable: true),
                    DocNumber = table.Column<string>(maxLength: 20, nullable: true),
                    TransportMode = table.Column<string>(nullable: true),
                    VessleNumber = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    AWBNumber = table.Column<string>(nullable: true),
                    AWBDate = table.Column<DateTime>(nullable: true),
                    DepartureDate = table.Column<DateTime>(nullable: true),
                    ArrivalDate = table.Column<DateTime>(nullable: true),
                    ShippingAgency = table.Column<string>(nullable: true),
                    GrossWeight = table.Column<double>(nullable: true),
                    GrossWeightUOM = table.Column<string>(nullable: true),
                    NetWeight = table.Column<double>(nullable: true),
                    NetWeightUOM = table.Column<string>(nullable: true),
                    VolumetricWeight = table.Column<double>(nullable: true),
                    VolumetricWeightUOM = table.Column<string>(nullable: true),
                    NumberOfPacks = table.Column<int>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    POBasicPrice = table.Column<double>(nullable: true),
                    TaxAmount = table.Column<double>(nullable: true),
                    InvoiceAmount = table.Column<double>(nullable: true),
                    InvoiceAmountUOM = table.Column<string>(nullable: true),
                    InvDocReferenceNo = table.Column<string>(nullable: true),
                    Plant = table.Column<string>(nullable: true),
                    PurchaseGroup = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsSubmitted = table.Column<bool>(nullable: false),
                    CancelDuration = table.Column<DateTime>(nullable: true),
                    ArrivalDateInterval = table.Column<int>(nullable: false),
                    BillOfLading = table.Column<string>(maxLength: 20, nullable: true),
                    TransporterName = table.Column<string>(maxLength: 40, nullable: true),
                    AccessibleValue = table.Column<double>(nullable: true),
                    ContactPerson = table.Column<string>(maxLength: 40, nullable: true),
                    ContactPersonNo = table.Column<string>(maxLength: 14, nullable: true),
                    Field1 = table.Column<string>(nullable: true),
                    Field2 = table.Column<string>(nullable: true),
                    Field3 = table.Column<string>(nullable: true),
                    Field4 = table.Column<string>(nullable: true),
                    Field5 = table.Column<string>(nullable: true),
                    Field6 = table.Column<string>(nullable: true),
                    Field7 = table.Column<string>(nullable: true),
                    Field8 = table.Column<string>(nullable: true),
                    Field9 = table.Column<string>(nullable: true),
                    Field10 = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_H1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I_Batch1",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    Batch = table.Column<string>(maxLength: 24, nullable: false),
                    Qty = table.Column<double>(nullable: false),
                    ManufactureDate = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    ManufactureCountry = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I_Batch1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.Item, x.Batch });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I_SES1",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    ServiceNo = table.Column<string>(maxLength: 24, nullable: false),
                    ServiceItem = table.Column<string>(nullable: true),
                    OrderedQty = table.Column<double>(nullable: false),
                    OpenQty = table.Column<double>(nullable: false),
                    ServiceQty = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I_SES1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.Item, x.ServiceNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I1",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(maxLength: 20, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    DeliveryDate = table.Column<DateTime>(nullable: true),
                    OrderedQty = table.Column<double>(nullable: false),
                    CompletedQty = table.Column<double>(nullable: false),
                    TransitQty = table.Column<double>(nullable: false),
                    OpenQty = table.Column<double>(nullable: false),
                    ASNQty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    HSN = table.Column<string>(nullable: true),
                    PlantCode = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<double>(nullable: true),
                    Value = table.Column<double>(nullable: true),
                    TaxAmount = table.Column<double>(nullable: true),
                    TaxCode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_OF_Map1",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_OF_Map1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_ASN_H1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_Batch1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_SES1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_OF_Map1");
        }
    }
}
