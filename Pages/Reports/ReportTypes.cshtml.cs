using MediSphere.Business.Interfaces;
using MediSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSphere.Pages.Reports
{
    [Authorize]
    public class ReportTypesModel : PageModel
    {
        private readonly IReportBusiness _reportBusiness;

        public IEnumerable<ReportTypeModel> ReportTypes { get; set; } = Enumerable.Empty<ReportTypeModel>();

        public ReportTypesModel(IReportBusiness reportBusiness)
        {
            _reportBusiness = reportBusiness;
        }

        public async Task OnGet()
        {
            ReportTypes = await _reportBusiness.GetReportTypesAsync();
        }
    }
}
