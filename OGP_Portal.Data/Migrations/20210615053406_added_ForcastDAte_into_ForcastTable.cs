using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OGP_Portal.Data.Migrations
{
    public partial class added_ForcastDAte_into_ForcastTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ForcastDate",
                table: "Forcast",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForcastDate",
                table: "Forcast");
        }
    }
}
