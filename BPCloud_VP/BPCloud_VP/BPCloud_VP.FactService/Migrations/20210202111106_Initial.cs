using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_AI_ACT",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    SeqNo = table.Column<int>(nullable: false),
                    AppID = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    ActionText = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    IsSeen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_AI_ACT", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SeqNo });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Cert",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    CertificateType = table.Column<string>(nullable: false),
                    CertificateName = table.Column<string>(nullable: false),
                    Validity = table.Column<DateTime>(nullable: true),
                    Mandatory = table.Column<string>(nullable: true),
                    Attachment = table.Column<string>(nullable: true),
                    AttachmentFile = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Cert", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.CertificateType, x.CertificateName });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Cert_Support",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    CertificateType = table.Column<string>(nullable: false),
                    CertificateName = table.Column<string>(nullable: false),
                    Validity = table.Column<DateTime>(nullable: true),
                    Mandatory = table.Column<string>(nullable: true),
                    Attachment = table.Column<string>(nullable: true),
                    AttachmentFile = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Cert_Support", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.CertificateType, x.CertificateName });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Dashboard_Card",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    AttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttachmentName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    AttachmentFile = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Dashboard_Card", x => x.AttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    LegalName = table.Column<string>(maxLength: 40, nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PinCode = table.Column<string>(nullable: true),
                    Plant = table.Column<string>(nullable: true),
                    GSTNumber = table.Column<string>(nullable: true),
                    GSTStatus = table.Column<string>(nullable: true),
                    PANNumber = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    Email1 = table.Column<string>(nullable: true),
                    Email2 = table.Column<string>(nullable: true),
                    TaxID1 = table.Column<string>(nullable: true),
                    TaxID2 = table.Column<string>(nullable: true),
                    OutstandingAmount = table.Column<double>(nullable: false),
                    CreditAmount = table.Column<double>(nullable: false),
                    LastPayment = table.Column<double>(nullable: false),
                    LastPaymentDate = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<double>(nullable: false),
                    CreditBalance = table.Column<double>(nullable: false),
                    CreditEvalDate = table.Column<DateTime>(nullable: true),
                    MSME = table.Column<bool>(nullable: false),
                    MSME_TYPE = table.Column<string>(nullable: true),
                    MSME_Att_ID = table.Column<string>(nullable: true),
                    Reduced_TDS = table.Column<bool>(nullable: false),
                    TDS_RATE = table.Column<string>(nullable: true),
                    TDS_Att_ID = table.Column<string>(nullable: true),
                    RP = table.Column<bool>(nullable: false),
                    RP_Name = table.Column<string>(nullable: true),
                    RP_Type = table.Column<string>(nullable: true),
                    RP_Att_ID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact", x => new { x.Client, x.Company, x.Type, x.PatnerID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_Bank",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    BankID = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_Bank", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.AccountNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_Bank_Support",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ID = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    BankID = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_Bank_Support", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.AccountNumber });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_CP",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    ContactPersonID = table.Column<string>(maxLength: 12, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_CP", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ContactPersonID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Fact_Support",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    LegalName = table.Column<string>(maxLength: 12, nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PinCode = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    Email1 = table.Column<string>(nullable: true),
                    Email2 = table.Column<string>(nullable: true),
                    TaxID1 = table.Column<string>(nullable: true),
                    TaxID2 = table.Column<string>(nullable: true),
                    OutstandingAmount = table.Column<double>(nullable: false),
                    CreditAmount = table.Column<double>(nullable: false),
                    LastPayment = table.Column<double>(nullable: false),
                    LastPaymentDate = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<double>(nullable: false),
                    CreditBalance = table.Column<double>(nullable: false),
                    CreditEvalDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Fact_Support", x => new { x.Client, x.Company, x.Type, x.PatnerID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_KRA",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    KRA = table.Column<string>(maxLength: 2, nullable: false),
                    EvalDate = table.Column<DateTime>(nullable: true),
                    KRAText = table.Column<string>(nullable: true),
                    KRAValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_KRA", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.KRA });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_AI_ACT");

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
