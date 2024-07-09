using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.POService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_ASN_H",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    ASNNumber = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    TransportMode = table.Column<string>(nullable: true),
                    VessleNumber = table.Column<string>(nullable: true),
                    CountryOfOrigin = table.Column<string>(nullable: true),
                    AWBNumber = table.Column<string>(nullable: true),
                    AWBDate = table.Column<DateTime>(nullable: true),
                    DepatDate = table.Column<DateTime>(nullable: true),
                    ArriveDate = table.Column<DateTime>(nullable: true),
                    CrossWeight = table.Column<double>(nullable: false),
                    NetWeight = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    VolumetricWeight = table.Column<double>(nullable: false),
                    VolumetricWeightUOM = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_H", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    ASNNumber = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    DelDate = table.Column<DateTime>(nullable: true),
                    OrderedQty = table.Column<double>(nullable: false),
                    CompletedQty = table.Column<double>(nullable: false),
                    TransitQty = table.Column<double>(nullable: false),
                    ASNQty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    ManfDate = table.Column<DateTime>(nullable: true),
                    ManfCountry = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_Cost",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    FLIPID = table.Column<string>(nullable: true),
                    ExpenceType = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_Cost", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_H",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    FLIPID = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    InvoiceAmount = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_H", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_I",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    FLIPID = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    DelDate = table.Column<DateTime>(nullable: true),
                    OrderedQty = table.Column<double>(nullable: false),
                    OpenQty = table.Column<double>(nullable: false),
                    InvoiceQty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    HSN = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_I", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_GRGI",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    GRGIDoc = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    DelDate = table.Column<DateTime>(nullable: true),
                    GRGIQty = table.Column<double>(nullable: false),
                    ShippingPartner = table.Column<string>(nullable: true),
                    ShippingDoc = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_GRGI", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_H",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    DocDate = table.Column<DateTime>(nullable: true),
                    DocVersion = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    CrossAmount = table.Column<double>(nullable: false),
                    NetAmount = table.Column<double>(nullable: false),
                    RefDoc = table.Column<string>(nullable: true),
                    AckStatus = table.Column<string>(nullable: true),
                    AckRemark = table.Column<string>(nullable: true),
                    AckDate = table.Column<DateTime>(nullable: true),
                    AckUser = table.Column<string>(nullable: true),
                    PINNumber = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_H", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_I",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    DelDate = table.Column<DateTime>(nullable: true),
                    OrderedQty = table.Column<double>(nullable: false),
                    CompletedQty = table.Column<double>(nullable: false),
                    TransitQty = table.Column<double>(nullable: false),
                    OpenQty = table.Column<double>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    HSN = table.Column<string>(nullable: true),
                    IsClosed = table.Column<bool>(nullable: false),
                    AckStatus = table.Column<string>(nullable: true),
                    AckDelDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_I", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_QM",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    GRGIQty = table.Column<double>(nullable: false),
                    LotQty = table.Column<double>(nullable: false),
                    RejQty = table.Column<double>(nullable: false),
                    RejReason = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_QM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_SL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PatnerID = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    SlLine = table.Column<string>(nullable: true),
                    DelDate = table.Column<DateTime>(nullable: true),
                    OrderedQty = table.Column<double>(nullable: false),
                    AckStatus = table.Column<string>(nullable: true),
                    AckDelDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_SL", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_ASN_H_Client_Company_Type_PatnerID_ASNNumber",
                table: "BPC_ASN_H",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "ASNNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_ASN_I_Client_Company_Type_PatnerID_ASNNumber_Item",
                table: "BPC_ASN_I",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "ASNNumber", "Item" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_FLIP_Cost_Client_Company_Type_PatnerID_FLIPID_ExpenceType",
                table: "BPC_FLIP_Cost",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "FLIPID", "ExpenceType" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_FLIP_H_Client_Company_Type_PatnerID_FLIPID",
                table: "BPC_FLIP_H",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "FLIPID" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_FLIP_I_Client_Company_Type_PatnerID_FLIPID_Item",
                table: "BPC_FLIP_I",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "FLIPID", "Item" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_OF_GRGI_Client_Company_Type_PatnerID_DocNumber_GRGIDoc_Item",
                table: "BPC_OF_GRGI",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber", "GRGIDoc", "Item" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_OF_H_Client_Company_Type_PatnerID_DocNumber",
                table: "BPC_OF_H",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_OF_I_Client_Company_Type_PatnerID_DocNumber_Item",
                table: "BPC_OF_I",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber", "Item" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_OF_QM_Client_Company_Type_PatnerID_DocNumber_Item",
                table: "BPC_OF_QM",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber", "Item" });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_OF_SL_Client_Company_Type_PatnerID_DocNumber_Item_SlLine",
                table: "BPC_OF_SL",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber", "Item", "SlLine" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_ASN_H");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_Cost");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_H");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_I");

            migrationBuilder.DropTable(
                name: "BPC_OF_GRGI");

            migrationBuilder.DropTable(
                name: "BPC_OF_H");

            migrationBuilder.DropTable(
                name: "BPC_OF_I");

            migrationBuilder.DropTable(
                name: "BPC_OF_QM");

            migrationBuilder.DropTable(
                name: "BPC_OF_SL");
        }
    }
}
