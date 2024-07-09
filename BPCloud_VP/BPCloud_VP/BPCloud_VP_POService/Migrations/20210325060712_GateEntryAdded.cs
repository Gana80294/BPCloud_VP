using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class GateEntryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BPC_GateEntry",
                columns: table => new
                {
                    Client = table.Column<string>(maxLength: 3, nullable: false),
                    Company = table.Column<string>(maxLength: 4, nullable: false),
                    Type = table.Column<string>(maxLength: 1, nullable: false),
                    PatnerID = table.Column<string>(maxLength: 12, nullable: false),
                    GateEntryNo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ASNNumber = table.Column<string>(nullable: false),
                    DocNumber = table.Column<string>(nullable: false),
                    VessleNumber = table.Column<string>(nullable: true),
                    GateEntryTime = table.Column<DateTime>(nullable: true),
                    ASNQty = table.Column<double>(nullable: false),
                    DepartureDate = table.Column<DateTime>(nullable: true),
                    ArrivalDate = table.Column<DateTime>(nullable: true),
                    Plant = table.Column<string>(nullable: true),
                    Transporter = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BPC_GateEntry", x => new { x.Client, x.Company, x.Type, x.PatnerID, x.ASNNumber, x.DocNumber, x.GateEntryNo });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BPC_GateEntry");
        }
    }
}
