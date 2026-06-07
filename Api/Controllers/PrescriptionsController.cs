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
    public class PrescriptionsController : ControllerBase
    {
        private readonly IRepository<PrescriptionModel> _repository;
        private readonly ILogger<PrescriptionsController> _logger;

        public PrescriptionsController(IRepository<PrescriptionModel> repository, ILogger<PrescriptionsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetAll()
        {
            var prescriptions = await _repository.GetAsync();
            return Ok(prescriptions.Select(MapToDto));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PrescriptionDto>> GetById(int id)
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

        [HttpGet("patient/{patientId:int}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetByPatient(int patientId)
        {
            var prescriptions = await _repository.GetAsync();
            return Ok(prescriptions.Where(p => p.PatientId == patientId).Select(MapToDto));
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionDto>> Create([FromBody] CreatePrescriptionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prescription = new PrescriptionModel
            {
                PatientId = dto.PatientId,
                MedicationName = dto.MedicationName,
                Dosage = dto.Dosage,
                PaymentNeeded = dto.PaymentNeeded,
                Notes = dto.Notes
            };

            var created = await _repository.CreateAsync(prescription);
            _logger.LogInformation("Prescription {PrescriptionId} created via API", created.PrescriptionId);
            return CreatedAtAction(nameof(GetById), new { id = created.PrescriptionId }, MapToDto(created));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PrescriptionDto>> Update(int id, [FromBody] CreatePrescriptionDto dto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                existing.PatientId = dto.PatientId;
                existing.MedicationName = dto.MedicationName;
                existing.Dosage = dto.Dosage;
                existing.PaymentNeeded = dto.PaymentNeeded;
                existing.Notes = dto.Notes;

                var updated = await _repository.UpdateAsync(existing);
                return Ok(MapToDto(updated));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var prescription = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(prescription);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private static PrescriptionDto MapToDto(PrescriptionModel prescription) => new()
        {
            PrescriptionId = prescription.PrescriptionId,
            PatientId = prescription.PatientId,
            MedicationName = prescription.MedicationName,
            Dosage = prescription.Dosage,
            PaymentNeeded = prescription.PaymentNeeded,
            Notes = prescription.Notes
        };
    }
}
