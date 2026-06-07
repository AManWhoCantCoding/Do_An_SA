using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocDocGo.Migrations
{
    public partial class ReportTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReportCreationTime",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReportStatus",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StaffName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportCreationTime",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportStatus",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "StaffName",
                table: "Reports");
        }
    }
}
