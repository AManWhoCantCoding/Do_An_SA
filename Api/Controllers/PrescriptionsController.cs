using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthSchemes.JwtOrCookie)]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionBusiness _prescriptionBusiness;
        private readonly ILogger<PrescriptionsController> _logger;

        public PrescriptionsController(IPrescriptionBusiness prescriptionBusiness, ILogger<PrescriptionsController> logger)
        {
            _prescriptionBusiness = prescriptionBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAll()
        {
            return Ok(await _prescriptionBusiness.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PrescriptionDto>> GetById(int id)
        {
            var prescription = await _prescriptionBusiness.GetByIdAsync(id);
            return prescription == null ? NotFound() : Ok(prescription);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByPatient(int patientId)
        {
            return Ok(await _prescriptionBusiness.GetByPatientIdAsync(patientId));
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> Create([FromBody] CreatePrescriptionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _prescriptionBusiness.CreateAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            _logger.LogInformation("Prescription {PrescriptionId} created via API", result.Data!.PrescriptionId);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.PrescriptionId }, result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PrescriptionDto>> Update(int id, [FromBody] CreatePrescriptionDto dto)
        {
            var result = await _prescriptionBusiness.UpdateAsync(id, dto);
            if (!result.Success)
            {
                return result.ErrorMessage == "Prescription not found." ? NotFound() : BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _prescriptionBusiness.DeleteAsync(id);
            return result.Success ? NoContent() : NotFound();
        }
    }
}
