using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocDocGo.Migrations
{
    public partial class AddDBSetReportTypesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypeModel_ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportTypeModel",
                table: "ReportTypeModel");

            migrationBuilder.RenameTable(
                name: "ReportTypeModel",
                newName: "ReportTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportTypes",
                table: "ReportTypes",
                column: "ReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeModelReportTypeId",
                table: "Reports",
                column: "ReportTypeModelReportTypeId",
                principalTable: "ReportTypes",
                principalColumn: "ReportTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportTypes",
                table: "ReportTypes");

            migrationBuilder.RenameTable(
                name: "ReportTypes",
                newName: "ReportTypeModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportTypeModel",
                table: "ReportTypeModel",
                column: "ReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypeModel_ReportTypeModelReportTypeId",
                table: "Reports",
                column: "ReportTypeModelReportTypeId",
                principalTable: "ReportTypeModel",
                principalColumn: "ReportTypeId");
        }
    }
}
