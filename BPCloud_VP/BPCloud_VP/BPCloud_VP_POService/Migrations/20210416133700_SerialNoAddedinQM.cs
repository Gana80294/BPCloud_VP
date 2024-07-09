using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud_VP_POService.Migrations
{
    public partial class SerialNoAddedinQM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BPC_OF_QM",
                table: "BPC_OF_QM");

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "BPC_OF_QM",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BPC_OF_QM",
                table: "BPC_OF_QM",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber", "Item", "SerialNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BPC_OF_QM",
                table: "BPC_OF_QM");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "BPC_OF_QM");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BPC_OF_QM",
                table: "BPC_OF_QM",
                columns: new[] { "Client", "Company", "Type", "PatnerID", "DocNumber", "Item" });
        }
    }
}
