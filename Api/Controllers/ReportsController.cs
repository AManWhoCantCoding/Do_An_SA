using DocDocGo.Dto;
using DocDocGo.Models;
using DocDocGo.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocDocGo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthSchemes.JwtOrCookie)]
    public class ReportsController : ControllerBase
    {
        private readonly IRepository<ReportModel> _repository;

        public ReportsController(IRepository<ReportModel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAll()
        {
            var reports = await _repository.GetAsync();
            return Ok(reports.Select(MapToDto));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReportDto>> GetById(int id)
        {
            try
            {
                return Ok(MapToDto(await _repository.GetByIdAsync(id)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private static ReportDto MapToDto(ReportModel report) => new()
        {
            ReportId = report.ReportId,
            PatientId = report.PatientId,
            ReportDescription = report.ReportDescription,
            InitialStaffName = report.InitialStaffName,
            CreatedAt = report.CreatedAt,
            LastUpdated = report.LastUpdated,
            LastUpdatedBy = report.LastUpdatedBy,
            Status = report.Status,
            IsReportPrinted = report.IsReportPrinted,
            ReportTypeId = report.ReportTypeId,
            ReportTypeName = report.ReportTypeModel?.TemplateType
        };
    }
}
