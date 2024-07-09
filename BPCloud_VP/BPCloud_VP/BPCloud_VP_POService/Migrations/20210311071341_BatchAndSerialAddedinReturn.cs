using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class BatchAndSerialAddedinReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_Ret_I_Batch",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(maxLength: 12, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    Batch = table.Column<string>(maxLength: 24, nullable: false),
                    OrderQty = table.Column<double>(nullable: false),
                    RetQty = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_I_Batch", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID, x.Item, x.Batch });
                });

            migrationBuilder.CreateTable(
                name: "BPC_Ret_I_Serial",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    RetReqID = table.Column<string>(maxLength: 12, nullable: false),
                    Item = table.Column<string>(maxLength: 4, nullable: false),
                    Serial = table.Column<string>(maxLength: 24, nullable: false),
                    OrderQty = table.Column<double>(nullable: false),
                    RetQty = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_Ret_I_Serial", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.RetReqID, x.Item, x.Serial });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_Ret_I_Batch");

            migrationBuilder.DropTable(
                name: "BPC_Ret_I_Serial");
        }
    }
}
