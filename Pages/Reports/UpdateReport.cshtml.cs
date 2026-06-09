using MediSphere.Business.Interfaces;
using MediSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSphere.Pages.Reports
{
    [Authorize]
    public class UpdateReportModel : PageModel
    {
        private readonly IReportBusiness _reportBusiness;

        public IEnumerable<PatientModel> Patients { get; set; } = Enumerable.Empty<PatientModel>();

        public UpdateReportModel(IReportBusiness reportBusiness)
        {
            _reportBusiness = reportBusiness;
        }

        [BindProperty]
        public ReportModel ExistingReportModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Patients = await _reportBusiness.GetPatientsForReportsAsync();
            var report = await _reportBusiness.GetByIdAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            ExistingReportModel = new ReportModel
            {
                ReportId = report.ReportId,
                PatientId = report.PatientId,
                ReportDescription = report.ReportDescription,
                InitialStaffName = report.InitialStaffName,
                Status = report.Status,
                CreatedAt = report.CreatedAt,
                LastUpdated = report.LastUpdated,
                LastUpdatedBy = report.LastUpdatedBy,
                IsReportPrinted = report.IsReportPrinted,
                ReportTypeId = report.ReportTypeId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Patients = await _reportBusiness.GetPatientsForReportsAsync();
                return Page();
            }

            var result = await _reportBusiness.UpdateReportAsync(ExistingReportModel, User.Identity?.Name);
            if (!result.Success)
            {
                return result.ErrorMessage == "Report not found." ? NotFound() : Page();
            }

            return RedirectToPage("/Reports/Index");
        }
    }
}
