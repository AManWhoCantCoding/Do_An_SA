using DocDocGo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocDocGo.Repositories.Interfaces;

namespace DocDocGo.Pages.Reports
{

    [Authorize]
    public class UpdateReportModel : PageModel
    {

        private readonly IRepository<ReportModel> _dbContext;
        private readonly IRepository<PatientModel> _patientcontext;

        public IEnumerable<PatientModel> Patients { get; set; }

        public UpdateReportModel(IRepository<ReportModel> dbContext, IRepository<PatientModel> patientDbContext)
        {
            _dbContext = dbContext;
            _patientcontext = patientDbContext;
        }

        [BindProperty]
        public ReportModel ExistingReportModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Patients = await _patientcontext.GetAsync();
            ExistingReportModel = await _dbContext.GetByIdAsync(id);

            if (ExistingReportModel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingReport = await _dbContext.GetByIdAsync(ExistingReportModel.ReportId);
            if (existingReport == null)
            {
                return NotFound();
            }

            string currentUser = User.Identity.Name;

            existingReport.ReportDescription = ExistingReportModel.ReportDescription;
            existingReport.PatientId = ExistingReportModel.PatientId;
            existingReport.InitialStaffName = ExistingReportModel.InitialStaffName;
            existingReport.LastUpdated = DateTime.Now;
            existingReport.LastUpdatedBy = currentUser;
            existingReport.Status = ExistingReportModel.Status;


            await _dbContext.UpdateAsync(existingReport);

            return RedirectToPage("/Reports/Index");

        }
    }

    }
