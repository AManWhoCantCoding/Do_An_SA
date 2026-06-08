using MediSphere.Dto;
using MediSphere.Models;
using MediSphere.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthSchemes.JwtOrCookie)]
    public class PatientsController : ControllerBase
    {
        private readonly IRepository<PatientModel> _repository;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(IRepository<PatientModel> repository, ILogger<PatientsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            var patients = await _repository.GetAsync();
            return Ok(patients.Select(MapToDto));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            try
            {
                var patient = await _repository.GetByIdAsync(id);
                return Ok(MapToDto(patient));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = new PatientModel
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactNumber = dto.ContactNumber,
                EmailAddress = dto.EmailAddress,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                IsPrivatePatient = dto.IsPrivatePatient
            };

            var created = await _repository.CreateAsync(patient);
            _logger.LogInformation("Patient {PatientId} created via API", created.PatientId);
            return CreatedAtAction(nameof(GetById), new { id = created.PatientId }, MapToDto(created));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PatientDto>> Update(int id, [FromBody] CreatePatientDto dto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                existing.FirstName = dto.FirstName;
                existing.LastName = dto.LastName;
                existing.ContactNumber = dto.ContactNumber;
                existing.EmailAddress = dto.EmailAddress;
                existing.DateOfBirth = dto.DateOfBirth;
                existing.Gender = dto.Gender;
                existing.IsPrivatePatient = dto.IsPrivatePatient;

                var updated = await _repository.UpdateAsync(existing);
                return Ok(MapToDto(updated));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var patient = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(patient);
                _logger.LogInformation("Patient {PatientId} deleted via API", id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private static PatientDto MapToDto(PatientModel patient) => new()
        {
            PatientId = patient.PatientId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            ContactNumber = patient.ContactNumber,
            EmailAddress = patient.EmailAddress,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            IsPrivatePatient = patient.IsPrivatePatient
        };
    }
}
