using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediSphere.Migrations
{
    public partial class AddReportType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ReportTypes",
                columns: new[] { "ReportTypeId", "ReportTypeCreationTime", "TemplateType" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical Examination Report" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hospital Revenue Report" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Doctor Shift & Duty Report" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medication Dispensation Report" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient Admission Statistics Report" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Clinical Laboratory Test Report" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical Supplies & Consumables Report" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Clinical Research & Trials Report" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ReportTypes",
                keyColumn: "ReportTypeId",
                keyValue: 8);
        }
    }
}
