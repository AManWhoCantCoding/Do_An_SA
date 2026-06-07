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
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository<AppointmentModel> _repository;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentRepository<AppointmentModel> repository, ILogger<AppointmentsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {
            var appointments = await _repository.GetAsync();
            return Ok(appointments.Select(MapToDto));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
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

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid || dto.StartTime >= dto.EndTime)
            {
                return BadRequest("Start time must be before end time.");
            }

            var appointment = new AppointmentModel
            {
                PatientId = dto.PatientId,
                Topic = dto.Topic,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status,
                Notes = dto.Notes
            };

            var created = await _repository.CreateAsync(appointment);
            _logger.LogInformation("Appointment {AppointmentId} created via API", created.AppointmentId);
            return CreatedAtAction(nameof(GetById), new { id = created.AppointmentId }, MapToDto(created));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<AppointmentDto>> Update(int id, [FromBody] CreateAppointmentDto dto)
        {
            if (dto.StartTime >= dto.EndTime)
            {
                return BadRequest("Start time must be before end time.");
            }

            try
            {
                var existing = await _repository.GetByIdAsync(id);
                existing.PatientId = dto.PatientId;
                existing.Topic = dto.Topic;
                existing.StartTime = dto.StartTime;
                existing.EndTime = dto.EndTime;
                existing.Status = dto.Status;
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
                var appointment = await _repository.GetByIdAsync(id);
                await _repository.DeleteAsync(appointment);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private static AppointmentDto MapToDto(AppointmentModel appointment) => new()
        {
            AppointmentId = appointment.AppointmentId,
            PatientId = appointment.PatientId,
            Topic = appointment.Topic,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
            Status = appointment.Status,
            Notes = appointment.Notes
        };
    }
}
