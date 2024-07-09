using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class ReturnTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_Ret_H",
                columns: table => new
                {
                   
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(maxLength: 12, nullable: false),
                    DocumentNumber = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    InvoiceDoc = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_H", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Ret_I",
                columns: table => new
                {
                    
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(maxLength: 12, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    ProdcutID = table.Column<string>(nullable: true),
                    Material = table.Column<string>(nullable: true),
                    MaterialText = table.Column<string>(nullable: true),
                    OrderQty = table.Column<double>(nullable: false),
                    RetQty = table.Column<double>(nullable: false),
                    ReasonText = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    AttachmentReferenceNo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_I", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID, x.Item });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_Ret_H");

            migrationBuilder.DropTable(
                name: "BPC_Ret_I");
        }
    }
}
