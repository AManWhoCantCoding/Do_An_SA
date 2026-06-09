using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using MediSphere.Models;
using MediSphere.Services.Interfaces;

namespace MediSphere.Business
{
    public class AppointmentBusiness : IAppointmentBusiness
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly INotificationService _notificationService;

        public AppointmentBusiness(
            IAppointmentService appointmentService,
            IPatientService patientService,
            INotificationService notificationService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentService.GetAllAsync();
            return appointments.Select(MapToDto);
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            try
            {
                return MapToDto(await _appointmentService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public async Task<BusinessResult<AppointmentDto>> CreateAsync(CreateAppointmentDto dto)
        {
            var validation = await ValidateAppointmentAsync(dto);
            if (!validation.Success)
            {
                return validation;
            }

            var appointment = new AppointmentModel
            {
                PatientId = dto.PatientId,
                Topic = dto.Topic,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status ?? "Scheduled",
                Notes = dto.Notes
            };

            var created = await _appointmentService.CreateAsync(appointment);

            try
            {
                var patient = await _patientService.GetByIdAsync(dto.PatientId);
                var patientName = $"{patient.FirstName} {patient.LastName}".Trim();
                await _notificationService.SendAppointmentConfirmationAsync(
                    patient.EmailAddress ?? string.Empty,
                    patientName,
                    created.StartTime,
                    created.Topic ?? "Appointment");
            }
            catch (Exception)
            {
                // Email failure must not block appointment creation.
            }

            return BusinessResult<AppointmentDto>.Ok(MapToDto(created));
        }

        public async Task<BusinessResult<AppointmentDto>> UpdateAsync(int id, CreateAppointmentDto dto)
        {
            var validation = await ValidateAppointmentAsync(dto);
            if (!validation.Success)
            {
                return validation;
            }

            try
            {
                var existing = await _appointmentService.GetByIdAsync(id);
                existing.PatientId = dto.PatientId;
                existing.Topic = dto.Topic;
                existing.StartTime = dto.StartTime;
                existing.EndTime = dto.EndTime;
                existing.Status = dto.Status;
                existing.Notes = dto.Notes;

                var updated = await _appointmentService.UpdateAsync(existing);
                return BusinessResult<AppointmentDto>.Ok(MapToDto(updated));
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<AppointmentDto>.Fail("Appointment not found.");
            }
        }

        public async Task<BusinessResult<bool>> DeleteAsync(int id)
        {
            try
            {
                await _appointmentService.DeleteAsync(id);
                return BusinessResult<bool>.Ok(true);
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<bool>.Fail("Appointment not found.");
            }
        }

        private async Task<BusinessResult<AppointmentDto>> ValidateAppointmentAsync(CreateAppointmentDto dto)
        {
            if (dto.StartTime >= dto.EndTime)
            {
                return BusinessResult<AppointmentDto>.Fail("Start time must be before end time.");
            }

            if (!await _patientService.ExistsAsync(dto.PatientId))
            {
                return BusinessResult<AppointmentDto>.Fail("Patient does not exist.");
            }

            return BusinessResult<AppointmentDto>.Ok(null!);
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
