using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.SupportDeskService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "BPC_FAQ_Attachment",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        FAQAttachmentID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        AttachmentName = table.Column<string>(nullable: true),
            //        ContentType = table.Column<string>(nullable: true),
            //        ContentLength = table.Column<long>(nullable: false),
            //        AttachmentFile = table.Column<byte[]>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_FAQ_Attachment", x => x.FAQAttachmentID);
            //    });

            migrationBuilder.CreateTable(
                name: "BPC_SAM",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    AppID = table.Column<string>(maxLength: 24, nullable: false),
                    AppName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SAM", x => new { x.Client, x.Company, x.Type, x.AppID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SH",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    SupportID = table.Column<string>(maxLength: 12, nullable: false),
                    ReasonCode = table.Column<string>(nullable: true),
                    Plant = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    DocumentRefNo = table.Column<string>(nullable: true),
                    AppID = table.Column<string>(nullable: true),
                    AttachmentID = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsResolved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SH", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SupportID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SL",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    SupportID = table.Column<string>(maxLength: 12, nullable: false),
                    SupportLogID = table.Column<string>(maxLength: 12, nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    AttachmentID = table.Column<string>(nullable: true),
                    IsResolved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SL", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.SupportID, x.SupportLogID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_SM",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    ReasonCode = table.Column<string>(maxLength: 24, nullable: false),
                    ReasonText = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_SM", x => new { x.Client, x.Company, x.Type, x.ReasonCode });
                });

            //migrationBuilder.CreateTable(
            //    name: "BPC_Support_Attachment",
            //    columns: table => new
            //    {
            //        IsActive = table.Column<bool>(nullable: false),
            //        CreatedOn = table.Column<DateTime>(nullable: false),
            //        CreatedBy = table.Column<string>(nullable: true),
            //        ModifiedOn = table.Column<DateTime>(nullable: true),
            //        ModifiedBy = table.Column<string>(nullable: true),
            //        AttachmentID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        SupportID = table.Column<string>(nullable: true),
            //        SupportLogID = table.Column<string>(nullable: true),
            //        AttachmentName = table.Column<string>(nullable: true),
            //        ContentType = table.Column<string>(nullable: true),
            //        ContentLength = table.Column<long>(nullable: false),
            //        AttachmentFile = table.Column<byte[]>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BPC_Support_Attachment", x => x.AttachmentID);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_SH_ReasonCode_Plant",
                table: "BPC_SH",
                columns: new[] { "ReasonCode", "Plant" });
        }

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
