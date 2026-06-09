using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthSchemes.JwtOrCookie)]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientBusiness _patientBusiness;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IPatientBusiness patientBusiness, ILogger<PatientsController> logger)
        {
            _patientBusiness = patientBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            return Ok(await _patientBusiness.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            var patient = await _patientBusiness.GetByIdAsync(id);
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _patientBusiness.CreateAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            _logger.LogInformation("Patient {PatientId} created via API", result.Data!.PatientId);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.PatientId }, result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PatientDto>> Update(int id, [FromBody] CreatePatientDto dto)
        {
            var result = await _patientBusiness.UpdateAsync(id, dto);
            if (!result.Success)
            {
                return result.ErrorMessage == "Patient not found." ? NotFound() : BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientBusiness.DeleteAsync(id);
            if (!result.Success)
            {
                return NotFound();
            }

            _logger.LogInformation("Patient {PatientId} deleted via API", id);
            return NoContent();
        }
    }
}
