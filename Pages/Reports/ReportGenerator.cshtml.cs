using DocDocGo.Models;
using DocDocGo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Reports
{
    [Authorize]
    public class ReportGeneratorModel : PageModel
    {
        private readonly IRepository<ReportModel> _dbContext;
        private readonly IRepository<ReportTypeModel> _reportTypeContext;
        private readonly IRepository<PatientModel> _patientcontext;

        public IEnumerable<PatientModel> Patients { get; set; }
        public IEnumerable<ReportTypeModel> ReportTypes { get; set; }

        public ReportGeneratorModel(IRepository<ReportModel> dbContext, IRepository<ReportTypeModel> typeDbContext, IRepository<PatientModel> patientDbContext)
        {
            _dbContext = dbContext;
            _reportTypeContext = typeDbContext;
            _patientcontext = patientDbContext;
        }

        [BindProperty]
        public ReportModel NewReportModel { get; set; }

        public ReportTypeModel SelectedReportType { get; set; }

        public async Task<IActionResult> OnGetAsync(int reportId)
        {
            Patients = await _patientcontext.GetAsync();
            ReportTypes = await _reportTypeContext.GetAsync();

            if (reportId != 0)
            {
                SelectedReportType = ReportTypes.FirstOrDefault(rt => rt.ReportTypeId == reportId);
                if (SelectedReportType != null)
                {
                    NewReportModel.ReportId = SelectedReportType.ReportTypeId;
                    NewReportModel.ReportDescription = SelectedReportType.TemplateType;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateReport()
        {
            if (ModelState.IsValid)
            {
               var ReportData = new ReportModel
                {
                    ReportDescription = NewReportModel.ReportDescription,
                    PatientId = NewReportModel.PatientId,
                    InitialStaffName = NewReportModel.InitialStaffName,
                    Status = NewReportModel.Status,
                    CreatedAt = DateTime.Now,
                    LastUpdated = DateTime.Now
                };

                await _dbContext.CreateAsync(ReportData);
                return RedirectToPage("/Reports/Index");

            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreateTemplateType(ReportTypeModel reporttype)
        {
            reporttype.TemplateType = NewReportModel.ReportDescription;
            reporttype.ReportTypeCreationTime = DateTime.Now;

            if (ModelState.IsValid)
            {

                await _reportTypeContext.CreateAsync(reporttype);

                return RedirectToPage("/Reports/ReportType");
            }
            return Page();
        }


    }
}
