using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediSphere.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthSchemes.JwtOrCookie)]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBusiness _appointmentBusiness;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentBusiness appointmentBusiness, ILogger<AppointmentsController> logger)
        {
            _appointmentBusiness = appointmentBusiness;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {
            return Ok(await _appointmentBusiness.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            var appointment = await _appointmentBusiness.GetByIdAsync(id);
            return appointment == null ? NotFound() : Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentBusiness.CreateAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            _logger.LogInformation("Appointment {AppointmentId} created via API", result.Data!.AppointmentId);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.AppointmentId }, result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<AppointmentDto>> Update(int id, [FromBody] CreateAppointmentDto dto)
        {
            var result = await _appointmentBusiness.UpdateAsync(id, dto);
            if (!result.Success)
            {
                return result.ErrorMessage == "Appointment not found." ? NotFound() : BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _appointmentBusiness.DeleteAsync(id);
            return result.Success ? NoContent() : NotFound();
        }
    }
}
