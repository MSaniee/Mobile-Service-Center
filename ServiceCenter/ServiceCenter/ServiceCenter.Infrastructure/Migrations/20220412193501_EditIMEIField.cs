using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCenter.Infrastructure.Migrations
{
    public partial class EditIMEIField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IMEI",
                table: "Receipts",
                newName: "Imei");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Imei",
                table: "Receipts",
                newName: "IMEI");
        }
    }
}
