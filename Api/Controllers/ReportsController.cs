using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthSchemes.JwtOrCookie)]
    public class ReportsController : ControllerBase
    {
        private readonly IReportBusiness _reportBusiness;

        public ReportsController(IReportBusiness reportBusiness)
        {
            _reportBusiness = reportBusiness;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAll()
        {
            return Ok(await _reportBusiness.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReportDto>> GetById(int id)
        {
            var report = await _reportBusiness.GetByIdAsync(id);
            return report == null ? NotFound() : Ok(report);
        }
    }
}
