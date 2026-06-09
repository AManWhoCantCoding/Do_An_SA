using ClosedXML.Excel;
using MediSphere.Business.Interfaces;
using MediSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSphere.Pages.Reports
{
    [Authorize]
    public class ExportReportModel : PageModel
    {
        private readonly IReportBusiness _reportBusiness;

        public ExportReportModel(IReportBusiness reportBusiness)
        {
            _reportBusiness = reportBusiness;
        }

        [BindProperty]
        public ReportModel ExistingReportModel { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string fileName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var report = await _reportBusiness.GetByIdAsync(Id);
            if (report == null)
            {
                return NotFound();
            }

            ExistingReportModel = new ReportModel
            {
                ReportId = report.ReportId,
                PatientId = report.PatientId,
                CreatedAt = report.CreatedAt,
                Status = report.Status,
                InitialStaffName = report.InitialStaffName,
                ReportTypeId = report.ReportTypeId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostExportToExcel()
        {
            var reportDto = await _reportBusiness.GetByIdAsync(Id);
            if (reportDto == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                ModelState.AddModelError("File Error", "Please enter a valid file name!");
                return Page();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                worksheet.Cell("A1").Value = "Report ID";
                worksheet.Cell("B1").Value = "Patient ID";
                worksheet.Cell("C1").Value = "Created On";
                worksheet.Cell("D1").Value = "Reporting Status";
                worksheet.Cell("E1").Value = "By Staff Member";
                worksheet.Cell("F1").Value = "Report Type";

                worksheet.Cell("A2").Value = reportDto.ReportId;
                worksheet.Cell("B2").Value = reportDto.PatientId;
                worksheet.Cell("C2").Value = reportDto.CreatedAt;
                worksheet.Cell("D2").Value = reportDto.Status;
                worksheet.Cell("E2").Value = reportDto.InitialStaffName;
                worksheet.Cell("F2").Value = reportDto.ReportTypeName ?? "N/A";

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                await _reportBusiness.MarkAsPrintedAsync(Id);

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
        }
    }
}
