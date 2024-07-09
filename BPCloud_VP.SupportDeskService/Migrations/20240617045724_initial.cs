using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BPCloud_VP.SupportDeskService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_FAQ_Attachment",
                columns: table => new
                {
                    FAQAttachmentID = table.Column<int>(type: "integer", nullable: false)
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
                    table.PrimaryKey("PK_BPC_FAQ_Attachment", x => x.FAQAttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "BPC_SAM",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    AppID = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    AppName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SAM", x => new { x.Client, x.Company, x.Type, x.AppID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SH",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SupportID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ReasonCode = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    Plant = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DocumentRefNo = table.Column<string>(type: "text", nullable: true),
                    AppID = table.Column<string>(type: "text", nullable: true),
                    AttachmentID = table.Column<string>(type: "text", nullable: true),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DocCount = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SH", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SupportID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SL",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SupportID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    SupportLogID = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    AttachmentID = table.Column<string>(type: "text", nullable: true),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SL", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SupportID, x.SupportLogID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SM",
                columns: table => new
                {
                    Client = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Company = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Type = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    ReasonCode = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ReasonText = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SM", x => new { x.Client, x.Company, x.Type, x.ReasonCode });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Support_Attachment",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupportID = table.Column<string>(type: "text", nullable: true),
                    SupportLogID = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_BPC_Support_Attachment", x => x.AttachmentID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_SH_ReasonCode_Plant",
                table: "BPC_SH",
                columns: new[] { "ReasonCode", "Plant" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_FAQ_Attachment");

            migrationBuilder.DropTable(
                name: "BPC_SAM");

            migrationBuilder.DropTable(
                name: "BPC_SH");

            migrationBuilder.DropTable(
                name: "BPC_SL");

            migrationBuilder.DropTable(
                name: "BPC_SM");

            migrationBuilder.DropTable(
                name: "BPC_Support_Attachment");
        }
    }
}
