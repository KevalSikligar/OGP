using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OGP_Portal.Data.Migrations
{
    public partial class added_EntryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    VendorName = table.Column<string>(nullable: true),
                    AsslyType = table.Column<int>(nullable: false),
                    ShellReceipt = table.Column<int>(nullable: false),
                    ShellWelding = table.Column<int>(nullable: false),
                    TopD_end_Receipt = table.Column<int>(nullable: false),
                    Fabrication = table.Column<int>(nullable: false),
                    Sent_to_paint = table.Column<int>(nullable: false),
                    Received_after_Paint = table.Column<int>(nullable: false),
                    Readiness = table.Column<int>(nullable: false),
                    BDE_Receipt = table.Column<int>(nullable: false),
                    BDE_readiness = table.Column<int>(nullable: false),
                    Shell_BDE = table.Column<int>(nullable: false),
                    Shell_BDE_Sent_to_Paint = table.Column<int>(nullable: false),
                    Shell_BDE_Received_after_paint = table.Column<int>(nullable: false),
                    SH_BDE_TDE = table.Column<int>(nullable: false),
                    Skid_Receipt = table.Column<int>(nullable: false),
                    Installation_Skid = table.Column<int>(nullable: false),
                    APT = table.Column<int>(nullable: false),
                    HPT = table.Column<int>(nullable: false),
                    Final_Paint = table.Column<int>(nullable: false),
                    Dispatch = table.Column<int>(nullable: false),
                    PartnerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entry_AspNetUsers_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entry_PartnerId",
                table: "Entry",
                column: "PartnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entry");
        }
    }
}
