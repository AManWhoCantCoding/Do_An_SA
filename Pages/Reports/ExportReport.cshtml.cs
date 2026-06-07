using ClosedXML.Excel;
using DocDocGo.Models;
using DocDocGo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Reports
{
    [Authorize]
    public class ExportReportModel : PageModel
    {
        private readonly IRepository<ReportModel> _dbContext;

        public ExportReportModel(IRepository<ReportModel> dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public ReportModel ExistingReportModel { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string fileName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ExistingReportModel = await _dbContext.GetByIdAsync(Id);

            if (ExistingReportModel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostExportToExcel()
        {
            var report = await _dbContext.GetByIdAsync(Id);

            if (report == null)
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

                worksheet.Cell("A2").Value = report.ReportId;
                worksheet.Cell("B2").Value = report.PatientId;
                worksheet.Cell("C2").Value = report.CreatedAt;
                worksheet.Cell("D2").Value = report.Status;
                worksheet.Cell("E2").Value = report.InitialStaffName;
                if(report.ReportTypeId == null)
                {
                    worksheet.Cell("F2").Value = "N/A";
              
                }
                else
                {
                    worksheet.Cell("F2").Value = report.ReportTypeId;

                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);

                stream.Position = 0;

                report.IsReportPrinted = true;

                await _dbContext.UpdateAsync(report);

                // return the Excel file as a FileStreamResult

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
            }
        }
    }
}
