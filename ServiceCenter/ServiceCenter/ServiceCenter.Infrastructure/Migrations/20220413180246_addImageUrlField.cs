using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceCenter.Infrastructure.Migrations
{
    public partial class addImageUrlField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Receipts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Receipts");
        }
    }
}
