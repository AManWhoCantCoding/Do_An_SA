using DocDocGo.Models;
using DocDocGo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Reports
{
    [Authorize]
    public class ReportTypesModel : PageModel
    {
        private IRepository<ReportTypeModel> _dbcontext;
        public IEnumerable<ReportTypeModel> ReportTypes { get; set; }

        public ReportTypesModel(IRepository<ReportTypeModel> dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task OnGet()
        {
            ReportTypes = await _dbcontext.GetAsync();

        }
    }
}
