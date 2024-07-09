using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BPCloud_VP.POService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BP_BC_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    BillAmount = table.Column<double>(type: "double precision", nullable: false),
                    PaidAmont = table.Column<double>(type: "double precision", nullable: false),
                    TDSAmount = table.Column<double>(type: "double precision", nullable: false),
                    TotalPaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    DownPayment = table.Column<double>(type: "double precision", nullable: false),
                    NetDueAmount = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    BalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    AcceptedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcceptedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BP_BC_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear });
                });

            migrationBuilder.CreateTable(
                name: "BP_BC_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    DocNumber = table.Column<string>(type: "text", nullable: false),
                    DocDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: false),
                    BillAmount = table.Column<double>(type: "double precision", nullable: false),
                    PaidAmont = table.Column<double>(type: "double precision", nullable: false),
                    TDSAmount = table.Column<double>(type: "double precision", nullable: false),
                    TotalPaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    DownPayment = table.Column<double>(type: "double precision", nullable: false),
                    NetDueAmount = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    BalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BP_BC_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FiscalYear, x.DocNumber });
                });

            migrationBuilder.CreateTable(
                name: "BP_Ticket_Status",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Ticket_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BP_Ticket_Status", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_Field_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    Field = table.Column<string>(type: "text", nullable: true),
                    FieldName = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    DefaultValue = table.Column<string>(type: "text", nullable: true),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Invisible = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_Field_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocNumber = table.Column<string>(type: "text", nullable: true),
                    TransportMode = table.Column<string>(type: "text", nullable: true),
                    VessleNumber = table.Column<string>(type: "text", nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "text", nullable: true),
                    AWBNumber = table.Column<string>(type: "text", nullable: true),
                    AWBDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippingAgency = table.Column<string>(type: "text", nullable: true),
                    GrossWeight = table.Column<double>(type: "double precision", nullable: true),
                    GrossWeightUOM = table.Column<string>(type: "text", nullable: true),
                    NetWeight = table.Column<double>(type: "double precision", nullable: true),
                    NetWeightUOM = table.Column<string>(type: "text", nullable: true),
                    VolumetricWeight = table.Column<double>(type: "double precision", nullable: true),
                    VolumetricWeightUOM = table.Column<string>(type: "text", nullable: true),
                    NumberOfPacks = table.Column<int>(type: "integer", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    POBasicPrice = table.Column<double>(type: "double precision", nullable: true),
                    TaxAmount = table.Column<double>(type: "double precision", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: true),
                    InvoiceAmountUOM = table.Column<string>(type: "text", nullable: true),
                    InvDocReferenceNo = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "boolean", nullable: false),
                    CancelDuration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalDateInterval = table.Column<int>(type: "integer", nullable: false),
                    BillOfLading = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TransporterName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    AccessibleValue = table.Column<double>(type: "double precision", nullable: true),
                    ContactPerson = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    ContactPersonNo = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    Field1 = table.Column<string>(type: "text", nullable: true),
                    Field2 = table.Column<string>(type: "text", nullable: true),
                    Field3 = table.Column<string>(type: "text", nullable: true),
                    Field4 = table.Column<string>(type: "text", nullable: true),
                    Field5 = table.Column<string>(type: "text", nullable: true),
                    Field6 = table.Column<string>(type: "text", nullable: true),
                    Field7 = table.Column<string>(type: "text", nullable: true),
                    Field8 = table.Column<string>(type: "text", nullable: true),
                    Field9 = table.Column<string>(type: "text", nullable: true),
                    Field10 = table.Column<string>(type: "text", nullable: true),
                    GateEntryNo = table.Column<string>(type: "text", nullable: true),
                    GateEntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GateEntryCreatedBy = table.Column<string>(type: "text", nullable: true),
                    IsBuyerApprovalRequired = table.Column<bool>(type: "boolean", nullable: false),
                    BuyerApprovalStatus = table.Column<string>(type: "text", nullable: true),
                    BuyerApprovalOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_H1",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TransportMode = table.Column<string>(type: "text", nullable: true),
                    VessleNumber = table.Column<string>(type: "text", nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "text", nullable: true),
                    AWBNumber = table.Column<string>(type: "text", nullable: true),
                    AWBDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippingAgency = table.Column<string>(type: "text", nullable: true),
                    GrossWeight = table.Column<double>(type: "double precision", nullable: true),
                    GrossWeightUOM = table.Column<string>(type: "text", nullable: true),
                    NetWeight = table.Column<double>(type: "double precision", nullable: true),
                    NetWeightUOM = table.Column<string>(type: "text", nullable: true),
                    VolumetricWeight = table.Column<double>(type: "double precision", nullable: true),
                    VolumetricWeightUOM = table.Column<string>(type: "text", nullable: true),
                    NumberOfPacks = table.Column<int>(type: "integer", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    POBasicPrice = table.Column<double>(type: "double precision", nullable: true),
                    TaxAmount = table.Column<double>(type: "double precision", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: true),
                    InvoiceAmountUOM = table.Column<string>(type: "text", nullable: true),
                    InvDocReferenceNo = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    PurchaseGroup = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "boolean", nullable: false),
                    CancelDuration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalDateInterval = table.Column<int>(type: "integer", nullable: false),
                    BillOfLading = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TransporterName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    AccessibleValue = table.Column<double>(type: "double precision", nullable: true),
                    ContactPerson = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    ContactPersonNo = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    Field1 = table.Column<string>(type: "text", nullable: true),
                    Field2 = table.Column<string>(type: "text", nullable: true),
                    Field3 = table.Column<string>(type: "text", nullable: true),
                    Field4 = table.Column<string>(type: "text", nullable: true),
                    Field5 = table.Column<string>(type: "text", nullable: true),
                    Field6 = table.Column<string>(type: "text", nullable: true),
                    Field7 = table.Column<string>(type: "text", nullable: true),
                    Field8 = table.Column<string>(type: "text", nullable: true),
                    Field9 = table.Column<string>(type: "text", nullable: true),
                    Field10 = table.Column<string>(type: "text", nullable: true),
                    GateEntryNo = table.Column<string>(type: "text", nullable: true),
                    GateEntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GateEntryCreatedBy = table.Column<string>(type: "text", nullable: true),
                    IsBuyerApprovalRequired = table.Column<bool>(type: "boolean", nullable: false),
                    BuyerApprovalStatus = table.Column<string>(type: "text", nullable: true),
                    BuyerApprovalOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_H1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: false),
                    CompletedQty = table.Column<double>(type: "double precision", nullable: false),
                    TransitQty = table.Column<double>(type: "double precision", nullable: false),
                    OpenQty = table.Column<double>(type: "double precision", nullable: false),
                    ASNQty = table.Column<double>(type: "double precision", nullable: false),
                    UOM = table.Column<string>(type: "text", nullable: false),
                    HSN = table.Column<string>(type: "text", nullable: true),
                    PlantCode = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    TaxAmount = table.Column<double>(type: "double precision", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    IsFreightAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    FreightAmount = table.Column<double>(type: "double precision", nullable: true),
                    ToleranceUpperLimit = table.Column<double>(type: "double precision", nullable: true),
                    ToleranceLowerLimit = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I_Batch",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Batch = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Qty = table.Column<double>(type: "double precision", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ManufactureCountry = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I_Batch", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.Item, x.Batch });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I_Batch1",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Batch = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Qty = table.Column<double>(type: "double precision", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ManufactureCountry = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I_Batch1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.Item, x.Batch });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I_SES",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    ServiceNo = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ServiceItem = table.Column<string>(type: "text", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: false),
                    OpenQty = table.Column<double>(type: "double precision", nullable: false),
                    ServiceQty = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I_SES", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.Item, x.ServiceNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I_SES1",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    ServiceNo = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ServiceItem = table.Column<string>(type: "text", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: false),
                    OpenQty = table.Column<double>(type: "double precision", nullable: false),
                    ServiceQty = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I_SES1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.Item, x.ServiceNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_I1",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: false),
                    CompletedQty = table.Column<double>(type: "double precision", nullable: false),
                    TransitQty = table.Column<double>(type: "double precision", nullable: false),
                    OpenQty = table.Column<double>(type: "double precision", nullable: false),
                    ASNQty = table.Column<double>(type: "double precision", nullable: false),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    HSN = table.Column<string>(type: "text", nullable: true),
                    PlantCode = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    TaxAmount = table.Column<double>(type: "double precision", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    IsFreightAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    FreightAmount = table.Column<double>(type: "double precision", nullable: true),
                    ToleranceUpperLimit = table.Column<double>(type: "double precision", nullable: true),
                    ToleranceLowerLimit = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_I1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_OF_Map1",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_OF_Map1", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_Pack",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ASNNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    PackageID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: true),
                    Dimension = table.Column<string>(type: "text", nullable: true),
                    GrossWeight = table.Column<double>(type: "double precision", nullable: true),
                    GrossWeightUOM = table.Column<string>(type: "text", nullable: true),
                    NetWeight = table.Column<double>(type: "double precision", nullable: true),
                    NetWeightUOM = table.Column<string>(type: "text", nullable: true),
                    VolumetricWeight = table.Column<double>(type: "double precision", nullable: true),
                    VolumetricWeightUOM = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_Pack", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.PackageID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ASN_PRE_SHIP_MASTER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PreShipmentDay = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ASN_PRE_SHIP_MASTER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_CEO_Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CEOMessage = table.Column<string>(type: "text", nullable: true),
                    IsReleased = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_CEO_Message", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Company_Master",
                columns: table => new
                {
                    Company = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Company_Master", x => x.Company);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Country_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Country_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Currency_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    CurrencyName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Currency_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_DocumentCenter",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ASNNumber = table.Column<string>(type: "text", nullable: true),
                    DocumentType = table.Column<string>(type: "text", nullable: true),
                    DocumentTitle = table.Column<string>(type: "text", nullable: true),
                    Filename = table.Column<string>(type: "text", nullable: true),
                    AttachmentReferenceNo = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_DocumentCenter", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_DocumentCenter_Master",
                columns: table => new
                {
                    AppID = table.Column<int>(type: "integer", nullable: false),
                    DocumentType = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Extension = table.Column<string>(type: "text", nullable: true),
                    SizeInKB = table.Column<double>(type: "double precision", nullable: false),
                    ForwardMail = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_DocumentCenter_Master", x => x.DocumentType);
                });

            migrationBuilder.CreateTable(
                name: "BPC_ExpenseType_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Client = table.Column<string>(type: "text", nullable: true),
                    Company = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    PatnerID = table.Column<string>(type: "text", nullable: true),
                    ExpenseType = table.Column<string>(type: "text", nullable: true),
                    MaxAmount = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ExpenseType_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_Attachment",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PO = table.Column<string>(type: "text", nullable: true),
                    FLIPID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    AttachmentName = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    ContentLength = table.Column<long>(type: "bigint", nullable: false),
                    AttachmentFile = table.Column<byte[]>(type: "bytea", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_Attachment", x => x.AttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_Cost",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FLIPID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ExpenceType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_Cost", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FLIPID, x.ExpenceType });
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FLIPID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    GSTIN = table.Column<string>(type: "text", nullable: true),
                    DocNumber = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    ProfitCentre = table.Column<string>(type: "text", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: true),
                    InvoiceCurrency = table.Column<string>(type: "text", nullable: true),
                    InvoiceType = table.Column<string>(type: "text", nullable: true),
                    InvoiceDocID = table.Column<string>(type: "text", nullable: true),
                    InvoiceAttachmentName = table.Column<string>(type: "text", nullable: true),
                    IsInvoiceOrCertified = table.Column<string>(type: "text", nullable: true),
                    IsInvoiceFlag = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FLIPID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_FLIP_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FLIPID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: true),
                    OpenQty = table.Column<double>(type: "double precision", nullable: true),
                    InvoiceQty = table.Column<double>(type: "double precision", nullable: false),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    HSN = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    TaxType = table.Column<string>(type: "text", nullable: true),
                    Tax = table.Column<double>(type: "double precision", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_FLIP_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.FLIPID, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Gate_HV",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Partner = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DocNo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Truck = table.Column<string>(type: "text", nullable: true),
                    Transporter = table.Column<string>(type: "text", nullable: true),
                    Gate = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Gate_HV", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNo, x.Partner });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Gate_TA",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Partner = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DocNo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Truck = table.Column<string>(type: "text", nullable: true),
                    Transporter = table.Column<string>(type: "text", nullable: true),
                    Gate = table.Column<string>(type: "text", nullable: true),
                    ExitDt = table.Column<string>(type: "text", nullable: true),
                    ExitTime = table.Column<string>(type: "text", nullable: true),
                    TATime = table.Column<string>(type: "text", nullable: true),
                    Exception = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Gate_TA", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNo, x.Partner });
                });

            migrationBuilder.CreateTable(
                name: "BPC_GateEntry",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    GateEntryNo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ASNNumber = table.Column<string>(type: "text", nullable: false),
                    DocNumber = table.Column<string>(type: "text", nullable: false),
                    VessleNumber = table.Column<string>(type: "text", nullable: true),
                    GateEntryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DepartureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    Transporter = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_GateEntry", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.GateEntryNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_GSTIN",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GSTIN = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_GSTIN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_HSN_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HSNCode = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_HSN_Master", x => x.ID);
                });

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
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: false),
                    AWBNumber = table.Column<string>(type: "text", nullable: true),
                    PoReference = table.Column<string>(type: "text", nullable: true),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    BalanceAmount = table.Column<double>(type: "double precision", nullable: true),
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
                    Item = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
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
                name: "BPC_LOG",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    APIMethod = table.Column<string>(type: "text", nullable: true),
                    NoOfRecords = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Response = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_LOG", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_AI_ACT",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SeqNo = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    AppID = table.Column<string>(type: "text", nullable: true),
                    DocNumber = table.Column<string>(type: "text", nullable: true),
                    ActionText = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Time = table.Column<string>(type: "text", nullable: true),
                    HasSeen = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_AI_ACT", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SeqNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_GRGI",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    GRGIDoc = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GRIDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GRGIQty = table.Column<double>(type: "double precision", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    ShippingPartner = table.Column<string>(type: "text", nullable: true),
                    ShippingDoc = table.Column<string>(type: "text", nullable: true),
                    IsFinal = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_GRGI", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.GRGIDoc, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DocType = table.Column<string>(type: "text", nullable: true),
                    DocDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocVersion = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    CrossAmount = table.Column<double>(type: "double precision", nullable: true),
                    NetAmount = table.Column<double>(type: "double precision", nullable: true),
                    RefDoc = table.Column<string>(type: "text", nullable: true),
                    AckStatus = table.Column<string>(type: "text", nullable: true),
                    AckRemark = table.Column<string>(type: "text", nullable: true),
                    AckDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AckUser = table.Column<string>(type: "text", nullable: true),
                    PINNumber = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    PlantName = table.Column<string>(type: "text", nullable: true),
                    POCreator = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    ReleasedStatus = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: true),
                    CompletedQty = table.Column<double>(type: "double precision", nullable: true),
                    TransitQty = table.Column<double>(type: "double precision", nullable: true),
                    OpenQty = table.Column<double>(type: "double precision", nullable: true),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    HSN = table.Column<string>(type: "text", nullable: true),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                    AckStatus = table.Column<string>(type: "text", nullable: true),
                    AckDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlantCode = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    TaxAmount = table.Column<double>(type: "double precision", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: true),
                    MaterialType = table.Column<string>(type: "text", nullable: true),
                    Flag = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    IsFreightAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    FreightAmount = table.Column<double>(type: "double precision", nullable: true),
                    ToleranceUpperLimit = table.Column<double>(type: "double precision", nullable: true),
                    ToleranceLowerLimit = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_I_SES",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    ServiceNo = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ServiceItem = table.Column<string>(type: "text", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: false),
                    OpenQty = table.Column<double>(type: "double precision", nullable: false),
                    ServiceQty = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_I_SES", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.ServiceNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_QM",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SerialNumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    GRGIQty = table.Column<double>(type: "double precision", nullable: true),
                    LotQty = table.Column<double>(type: "double precision", nullable: true),
                    RejQty = table.Column<double>(type: "double precision", nullable: true),
                    RejReason = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_QM", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.SerialNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_SL",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    SlLine = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: true),
                    OpenQty = table.Column<double>(type: "double precision", nullable: true),
                    AckStatus = table.Column<string>(type: "text", nullable: true),
                    AckDeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_SL", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.SlLine });
                });

            migrationBuilder.CreateTable(
                name: "BPC_OF_Subcon",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    SlLine = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderedQty = table.Column<double>(type: "double precision", nullable: false),
                    Batch = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_OF_Subcon", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.DocNumber, x.Item, x.SlLine });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_AS",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    DocumentNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: false),
                    BalanceAmount = table.Column<double>(type: "double precision", nullable: false),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    AdvanceAmount = table.Column<double>(type: "double precision", nullable: true),
                    TDS = table.Column<double>(type: "double precision", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    ProfitCenter = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    AcceptedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcceptedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_AS", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.DocumentNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_DIS",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    DocumentNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceAmount = table.Column<double>(type: "double precision", nullable: false),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    BalanceAmount = table.Column<double>(type: "double precision", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProposedDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProposedDiscount = table.Column<double>(type: "double precision", nullable: false),
                    PostDiscountAmount = table.Column<double>(type: "double precision", nullable: false),
                    ProfitCenter = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_DIS", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.DocumentNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_DIS_MASTER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<double>(type: "double precision", nullable: false),
                    ProfitCenter = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_DIS_MASTER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_PAYABLE",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Invoice = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PostedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdvAmount = table.Column<double>(type: "double precision", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_PAYABLE", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.Invoice });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_PAYMENT",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    DocumentNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentType = table.Column<string>(type: "text", nullable: true),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    BankName = table.Column<string>(type: "text", nullable: true),
                    BankAccount = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_PAYMENT", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.DocumentNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_RECORD",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocumentNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    InvoiceNumber = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PayRecordNo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PaidAmount = table.Column<double>(type: "double precision", nullable: false),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    Medium = table.Column<string>(type: "text", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Time = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    RefNumber = table.Column<string>(type: "text", nullable: true),
                    PayDoc = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_RECORD", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.DocumentNumber, x.InvoiceNumber, x.PayRecordNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PAY_TDS",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PartnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    FiscalYear = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    CompanyCode = table.Column<string>(type: "text", nullable: false),
                    DocumentID = table.Column<string>(type: "text", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BaseAmount = table.Column<double>(type: "double precision", nullable: false),
                    TDSCategory = table.Column<string>(type: "text", nullable: true),
                    TDSAmount = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PAY_TDS", x => new { x.Client, x.Company, x.Type, x.PartnerID, x.FiscalYear, x.CompanyCode, x.DocumentID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PI_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    PIRNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    PIRType = table.Column<string>(type: "text", nullable: true),
                    DocumentNumber = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReferenceDoc = table.Column<string>(type: "text", nullable: true),
                    GrossAmount = table.Column<double>(type: "double precision", nullable: true),
                    NetAmount = table.Column<double>(type: "double precision", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Qty = table.Column<double>(type: "double precision", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    DeliveryNote = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PI_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.PIRNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_PI_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    PIRNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    ProdcutID = table.Column<string>(type: "text", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrderQty = table.Column<double>(type: "double precision", nullable: true),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    HSN = table.Column<string>(type: "text", nullable: true),
                    RetQty = table.Column<double>(type: "double precision", nullable: true),
                    ReasonText = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    AttachmentReferenceNo = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_PI_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.PIRNumber, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Plant_Master",
                columns: table => new
                {
                    PlantCode = table.Column<string>(type: "text", nullable: false),
                    PlantText = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PinCode = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Plant_Master", x => x.PlantCode);
                });

            migrationBuilder.CreateTable(
                name: "BPC_POD_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: false),
                    DocNumber = table.Column<string>(type: "text", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TruckNumber = table.Column<string>(type: "text", nullable: true),
                    VessleNumber = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    Transporter = table.Column<string>(type: "text", nullable: true),
                    Driver = table.Column<string>(type: "text", nullable: true),
                    DriverContactNo = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_POD_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.InvoiceNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_POD_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: false),
                    Item = table.Column<string>(type: "text", nullable: false),
                    DocNumber = table.Column<string>(type: "text", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    Qty = table.Column<double>(type: "double precision", nullable: false),
                    ReceivedQty = table.Column<double>(type: "double precision", nullable: true),
                    BreakageQty = table.Column<double>(type: "double precision", nullable: true),
                    MissingQty = table.Column<double>(type: "double precision", nullable: true),
                    AcceptedQty = table.Column<double>(type: "double precision", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    AttachmentName = table.Column<string>(type: "text", nullable: true),
                    AttachmentReferenceNo = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_POD_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.InvoiceNumber, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Prod",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ProductID = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    MaterialType = table.Column<string>(type: "text", nullable: true),
                    MaterialGroup = table.Column<string>(type: "text", nullable: true),
                    AttID = table.Column<string>(type: "text", nullable: true),
                    UOM = table.Column<string>(type: "text", nullable: true),
                    Stock = table.Column<string>(type: "text", nullable: true),
                    BasePrice = table.Column<string>(type: "text", nullable: true),
                    StockUpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Prod", x => new { x.Client, x.Company, x.Type, x.ProductID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Prod_Fav",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ProductID = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Prod_Fav", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ProductID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_ProfitCentre_Master",
                columns: table => new
                {
                    ProfitCentre = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_ProfitCentre_Master", x => x.ProfitCentre);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Reason_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReasonCode = table.Column<string>(type: "text", nullable: true),
                    ReasonText = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Reason_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Ret_H",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    DocumentNumber = table.Column<string>(type: "text", nullable: true),
                    CreditNote = table.Column<string>(type: "text", nullable: true),
                    AWBNumber = table.Column<string>(type: "text", nullable: true),
                    Transporter = table.Column<string>(type: "text", nullable: true),
                    TruckNumber = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvoiceDoc = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Ret_I",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    ProdcutID = table.Column<string>(type: "text", nullable: true),
                    Material = table.Column<string>(type: "text", nullable: true),
                    MaterialText = table.Column<string>(type: "text", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "text", nullable: true),
                    OrderQty = table.Column<double>(type: "double precision", nullable: false),
                    RetQty = table.Column<double>(type: "double precision", nullable: false),
                    ReasonText = table.Column<string>(type: "text", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    AttachmentReferenceNo = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID, x.Item });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Ret_I_Batch",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Batch = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    RetQty = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_I_Batch", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID, x.Item, x.Batch });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Ret_I_Serial",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Item = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Serial = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    RetQty = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_I_Serial", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID, x.Item, x.Serial });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SCOC_Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SCOCMessage = table.Column<string>(type: "text", nullable: true),
                    IsReleased = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SCOC_Message", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_TaxType_Master",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaxType = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_TaxType_Master", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Welcome_Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WelcomeMessage = table.Column<string>(type: "text", nullable: true),
                    IsReleased = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Welcome_Message", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "BPCAttachments",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Client = table.Column<string>(type: "text", nullable: true),
                    Company = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    PatnerID = table.Column<string>(type: "text", nullable: true),
                    ReferenceNo = table.Column<string>(type: "text", nullable: true),
                    AttachmentName = table.Column<string>(type: "text", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: true),
                    ContentLength = table.Column<long>(type: "bigint", nullable: false),
                    AttachmentFile = table.Column<byte[]>(type: "bytea", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPCAttachments", x => x.AttachmentID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_ASN_Field_Master_Field",
                table: "BPC_ASN_Field_Master",
                column: "Field");

            migrationBuilder.CreateIndex(
                name: "IX_BPC_FLIP_Attachment_FLIPID",
                table: "BPC_FLIP_Attachment",
                column: "FLIPID");

            migrationBuilder.CreateIndex(
                name: "IX_BPC_PAY_DIS_ProfitCenter",
                table: "BPC_PAY_DIS",
                column: "ProfitCenter");

            migrationBuilder.CreateIndex(
                name: "IX_BPC_PAY_DIS_MASTER_FiscalYear_ProfitCenter",
                table: "BPC_PAY_DIS_MASTER",
                columns: new[] { "FiscalYear", "ProfitCenter" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BP_BC_H");

            migrationBuilder.DropTable(
                name: "BP_BC_I");

            migrationBuilder.DropTable(
                name: "BP_Ticket_Status");

            migrationBuilder.DropTable(
                name: "BPC_ASN_Field_Master");

            migrationBuilder.DropTable(
                name: "BPC_ASN_H");

            migrationBuilder.DropTable(
                name: "BPC_ASN_H1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_Batch");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_Batch1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_SES");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I_SES1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_I1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_OF_Map1");

            migrationBuilder.DropTable(
                name: "BPC_ASN_Pack");

            migrationBuilder.DropTable(
                name: "BPC_ASN_PRE_SHIP_MASTER");

            migrationBuilder.DropTable(
                name: "BPC_CEO_Message");

            migrationBuilder.DropTable(
                name: "BPC_Company_Master");

            migrationBuilder.DropTable(
                name: "BPC_Country_Master");

            migrationBuilder.DropTable(
                name: "BPC_Currency_Master");

            migrationBuilder.DropTable(
                name: "BPC_DocumentCenter");

            migrationBuilder.DropTable(
                name: "BPC_DocumentCenter_Master");

            migrationBuilder.DropTable(
                name: "BPC_ExpenseType_Master");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_Attachment");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_Cost");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_H");

            migrationBuilder.DropTable(
                name: "BPC_FLIP_I");

            migrationBuilder.DropTable(
                name: "BPC_Gate_HV");

            migrationBuilder.DropTable(
                name: "BPC_Gate_TA");

            migrationBuilder.DropTable(
                name: "BPC_GateEntry");

            migrationBuilder.DropTable(
                name: "BPC_GSTIN");

            migrationBuilder.DropTable(
                name: "BPC_HSN_Master");

            migrationBuilder.DropTable(
                name: "BPC_INV");

            migrationBuilder.DropTable(
                name: "BPC_INV_I");

            migrationBuilder.DropTable(
                name: "BPC_LOG");

            migrationBuilder.DropTable(
                name: "BPC_OF_AI_ACT");

            migrationBuilder.DropTable(
                name: "BPC_OF_GRGI");

            migrationBuilder.DropTable(
                name: "BPC_OF_H");

            migrationBuilder.DropTable(
                name: "BPC_OF_I");

            migrationBuilder.DropTable(
                name: "BPC_OF_I_SES");

            migrationBuilder.DropTable(
                name: "BPC_OF_QM");

            migrationBuilder.DropTable(
                name: "BPC_OF_SL");

            migrationBuilder.DropTable(
                name: "BPC_OF_Subcon");

            migrationBuilder.DropTable(
                name: "BPC_PAY_AS");

            migrationBuilder.DropTable(
                name: "BPC_PAY_DIS");

            migrationBuilder.DropTable(
                name: "BPC_PAY_DIS_MASTER");

            migrationBuilder.DropTable(
                name: "BPC_PAY_PAYABLE");

            migrationBuilder.DropTable(
                name: "BPC_PAY_PAYMENT");

            migrationBuilder.DropTable(
                name: "BPC_PAY_RECORD");

            migrationBuilder.DropTable(
                name: "BPC_PAY_TDS");

            migrationBuilder.DropTable(
                name: "BPC_PI_H");

            migrationBuilder.DropTable(
                name: "BPC_PI_I");

            migrationBuilder.DropTable(
                name: "BPC_Plant_Master");

            migrationBuilder.DropTable(
                name: "BPC_POD_H");

            migrationBuilder.DropTable(
                name: "BPC_POD_I");

            migrationBuilder.DropTable(
                name: "BPC_Prod");

            migrationBuilder.DropTable(
                name: "BPC_Prod_Fav");

            migrationBuilder.DropTable(
                name: "BPC_ProfitCentre_Master");

            migrationBuilder.DropTable(
                name: "BPC_Reason_Master");

            migrationBuilder.DropTable(
                name: "BPC_Ret_H");

            migrationBuilder.DropTable(
                name: "BPC_Ret_I");

            migrationBuilder.DropTable(
                name: "BPC_Ret_I_Batch");

            migrationBuilder.DropTable(
                name: "BPC_Ret_I_Serial");

            migrationBuilder.DropTable(
                name: "BPC_SCOC_Message");

            migrationBuilder.DropTable(
                name: "BPC_TaxType_Master");

            migrationBuilder.DropTable(
                name: "BPC_Welcome_Message");

            migrationBuilder.DropTable(
                name: "BPCAttachments");
        }
    }
}
