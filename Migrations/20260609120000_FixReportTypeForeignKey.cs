using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediSphere.Migrations
{
    public partial class FixReportTypeForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Reports
                SET ReportTypeId = ReportTypeModelReportTypeId
                WHERE ReportTypeId IS NULL AND ReportTypeModelReportTypeId IS NOT NULL
            ");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportTypeModelReportTypeId",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportTypeId",
                table: "Reports",
                column: "ReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeId",
                table: "Reports",
                column: "ReportTypeId",
                principalTable: "ReportTypes",
                principalColumn: "ReportTypeId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportTypeId",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "ReportTypeModelReportTypeId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Reports
                SET ReportTypeModelReportTypeId = ReportTypeId
                WHERE ReportTypeId IS NOT NULL
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportTypeModelReportTypeId",
                table: "Reports",
                column: "ReportTypeModelReportTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeModelReportTypeId",
                table: "Reports",
                column: "ReportTypeModelReportTypeId",
                principalTable: "ReportTypes",
                principalColumn: "ReportTypeId");
        }
    }
}
