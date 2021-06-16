using Microsoft.EntityFrameworkCore.Migrations;

namespace OGP_Portal.Data.Migrations
{
    public partial class added_ispassreset_filed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPasssReset",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPasssReset",
                table: "AspNetUsers");
        }
    }
}
