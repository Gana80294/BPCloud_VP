using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BPCloud_VP.FactService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_AI_ACT",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SeqNo = table.Column<int>(type: "integer", nullable: false),
                    ActType = table.Column<string>(type: "text", nullable: true),
                    AppID = table.Column<string>(type: "text", nullable: true),
                    DocNumber = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "text", nullable: true),
                    ActionText = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Time = table.Column<string>(type: "text", nullable: true),
                    IsSeen = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_AI_ACT", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SeqNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Attachments",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
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
                    table.PrimaryKey("PK_BPC_Attachments", x => x.AttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Cert",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CertificateType = table.Column<string>(type: "text", nullable: false),
                    CertificateName = table.Column<string>(type: "text", nullable: false),
                    Validity = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Mandatory = table.Column<string>(type: "text", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    AttachmentFile = table.Column<byte[]>(type: "bytea", nullable: true),
                    AttachmentID = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Cert", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.CertificateType, x.CertificateName });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Cert_Support",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    CertificateType = table.Column<string>(type: "text", nullable: false),
                    CertificateName = table.Column<string>(type: "text", nullable: false),
                    ID = table.Column<string>(type: "text", nullable: true),
                    Validity = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Mandatory = table.Column<string>(type: "text", nullable: true),
                    Attachment = table.Column<string>(type: "text", nullable: true),
                    AttachmentFile = table.Column<byte[]>(type: "bytea", nullable: true),
                    AttachmentID = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Cert_Support", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.CertificateType, x.CertificateName });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Dashboard_Card",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_BPC_Dashboard_Card", x => x.AttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PinCode = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    GSTNumber = table.Column<string>(type: "text", nullable: true),
                    GSTStatus = table.Column<string>(type: "text", nullable: true),
                    PANNumber = table.Column<string>(type: "text", nullable: true),
                    Phone1 = table.Column<string>(type: "text", nullable: true),
                    Phone2 = table.Column<string>(type: "text", nullable: true),
                    Email1 = table.Column<string>(type: "text", nullable: true),
                    Email2 = table.Column<string>(type: "text", nullable: true),
                    TaxID1 = table.Column<string>(type: "text", nullable: true),
                    TaxID2 = table.Column<string>(type: "text", nullable: true),
                    OutstandingAmount = table.Column<double>(type: "double precision", nullable: false),
                    CreditAmount = table.Column<double>(type: "double precision", nullable: false),
                    LastPayment = table.Column<double>(type: "double precision", nullable: false),
                    LastPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    CreditLimit = table.Column<double>(type: "double precision", nullable: false),
                    CreditBalance = table.Column<double>(type: "double precision", nullable: false),
                    CreditEvalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MSME = table.Column<bool>(type: "boolean", nullable: false),
                    MSME_TYPE = table.Column<string>(type: "text", nullable: true),
                    MSME_Att_ID = table.Column<string>(type: "text", nullable: true),
                    Reduced_TDS = table.Column<bool>(type: "boolean", nullable: false),
                    TDS_RATE = table.Column<string>(type: "text", nullable: true),
                    TDS_Att_ID = table.Column<string>(type: "text", nullable: true),
                    TDS_Cert_No = table.Column<string>(type: "text", nullable: true),
                    RP = table.Column<bool>(type: "boolean", nullable: false),
                    RP_Name = table.Column<string>(type: "text", nullable: true),
                    RP_Type = table.Column<string>(type: "text", nullable: true),
                    RP_Att_ID = table.Column<string>(type: "text", nullable: true),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    PurchaseOrg = table.Column<string>(type: "text", nullable: true),
                    AccountGroup = table.Column<string>(type: "text", nullable: true),
                    CompanyCode = table.Column<string>(type: "text", nullable: true),
                    TypeofIndustry = table.Column<string>(type: "text", nullable: true),
                    VendorCode = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact", x => new { x.Client, x.Company, x.Type, x.PatnerID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_Bank",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    AccountNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    BankID = table.Column<string>(type: "text", nullable: true),
                    BankName = table.Column<string>(type: "text", nullable: true),
                    IFSC = table.Column<string>(type: "text", nullable: true),
                    Branch = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_Bank", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.AccountNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_Bank_Support",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    AccountNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ID = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    BankID = table.Column<string>(type: "text", nullable: true),
                    BankName = table.Column<string>(type: "text", nullable: true),
                    IFSC = table.Column<string>(type: "text", nullable: true),
                    Branch = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_Bank_Support", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.AccountNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_CP",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ContactPersonID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ContactNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Item = table.Column<string>(type: "text", nullable: true),
                    Department = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Mobile = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_CP", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ContactPersonID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_Support",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    LegalName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    PinCode = table.Column<string>(type: "text", nullable: true),
                    Plant = table.Column<string>(type: "text", nullable: true),
                    GSTNumber = table.Column<string>(type: "text", nullable: true),
                    GSTStatus = table.Column<string>(type: "text", nullable: true),
                    PANNumber = table.Column<string>(type: "text", nullable: true),
                    Phone1 = table.Column<string>(type: "text", nullable: true),
                    Phone2 = table.Column<string>(type: "text", nullable: true),
                    Email1 = table.Column<string>(type: "text", nullable: true),
                    Email2 = table.Column<string>(type: "text", nullable: true),
                    TaxID1 = table.Column<string>(type: "text", nullable: true),
                    TaxID2 = table.Column<string>(type: "text", nullable: true),
                    OutstandingAmount = table.Column<double>(type: "double precision", nullable: false),
                    CreditAmount = table.Column<double>(type: "double precision", nullable: false),
                    LastPayment = table.Column<double>(type: "double precision", nullable: false),
                    LastPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    CreditLimit = table.Column<double>(type: "double precision", nullable: false),
                    CreditBalance = table.Column<double>(type: "double precision", nullable: false),
                    CreditEvalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MSME = table.Column<bool>(type: "boolean", nullable: false),
                    MSME_TYPE = table.Column<string>(type: "text", nullable: true),
                    MSME_Att_ID = table.Column<string>(type: "text", nullable: true),
                    Reduced_TDS = table.Column<bool>(type: "boolean", nullable: false),
                    TDS_RATE = table.Column<string>(type: "text", nullable: true),
                    TDS_Att_ID = table.Column<string>(type: "text", nullable: true),
                    TDS_Cert_No = table.Column<string>(type: "text", nullable: true),
                    RP = table.Column<bool>(type: "boolean", nullable: false),
                    RP_Name = table.Column<string>(type: "text", nullable: true),
                    RP_Type = table.Column<string>(type: "text", nullable: true),
                    RP_Att_ID = table.Column<string>(type: "text", nullable: true),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false),
                    PurchaseOrg = table.Column<string>(type: "text", nullable: true),
                    AccountGroup = table.Column<string>(type: "text", nullable: true),
                    CompanyCode = table.Column<string>(type: "text", nullable: true),
                    TypeofIndustry = table.Column<string>(type: "text", nullable: true),
                    VendorCode = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_Support", x => new { x.Client, x.Company, x.Type, x.PatnerID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_KRA",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    KRA = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    EvalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KRAText = table.Column<string>(type: "text", nullable: true),
                    KRAValue = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_KRA", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.KRA });
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_Attachments_Client_Company_Type_PatnerID",
                table: "BPC_Attachments",
                columns: new[] { "Client", "Company", "Type", "PatnerID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_AI_ACT");

            migrationBuilder.DropTable(
                name: "BPC_Attachments");

            migrationBuilder.DropTable(
                name: "BPC_Cert");

            migrationBuilder.DropTable(
                name: "BPC_Cert_Support");

            migrationBuilder.DropTable(
                name: "BPC_Dashboard_Card");

            migrationBuilder.DropTable(
                name: "BPC_Fact");

            migrationBuilder.DropTable(
                name: "BPC_Fact_Bank");

            migrationBuilder.DropTable(
                name: "BPC_Fact_Bank_Support");

            migrationBuilder.DropTable(
                name: "BPC_Fact_CP");

            migrationBuilder.DropTable(
                name: "BPC_Fact_Support");

            migrationBuilder.DropTable(
                name: "BPC_KRA");
        }
    }
}
