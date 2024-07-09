using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP.FactService.Migrations
{
    public partial class FactAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_Attachments",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Client = table.Column<string>(maxLength: 3, nullable: true),
                    Company = table.Column<string>(maxLength: 4, nullable: true),
                    Type = table.Column<string>(maxLength: 1, nullable: true),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: true),
                    ReferenceNo = table.Column<string>(nullable: true),
                    AttachmentName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    AttachmentFile = table.Column<byte[]>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Attachments", x => x.AttachmentID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BPC_Attachments_Client_Company_Type_PatnerID",
                table: "BPC_Attachments",
                columns: new[] { "Client", "Company", "Type", "PatnerID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_Attachments");
        }
    }
}
