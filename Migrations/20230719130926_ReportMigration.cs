using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocDocGo.Migrations
{
    public partial class ReportMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportStatus",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "StaffName",
                table: "Reports",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ReportType",
                table: "Reports",
                newName: "InitialStaffName");

            migrationBuilder.RenameColumn(
                name: "ReportCreationTime",
                table: "Reports",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Reports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportTypeId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportTypeModelReportTypeId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportTypeModel",
                columns: table => new
                {
                    ReportTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportTypeCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTypeModel", x => x.ReportTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportTypeModelReportTypeId",
                table: "Reports",
                column: "ReportTypeModelReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypeModel_ReportTypeModelReportTypeId",
                table: "Reports",
                column: "ReportTypeModelReportTypeId",
                principalTable: "ReportTypeModel",
                principalColumn: "ReportTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypeModel_ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "ReportTypeModel");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportTypeId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Reports",
                newName: "StaffName");

            migrationBuilder.RenameColumn(
                name: "InitialStaffName",
                table: "Reports",
                newName: "ReportType");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reports",
                newName: "ReportCreationTime");

            migrationBuilder.AddColumn<string>(
                name: "ReportStatus",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
