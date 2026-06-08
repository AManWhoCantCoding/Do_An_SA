using System.ComponentModel.DataAnnotations;

namespace MediSphere.Dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string? Topic { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }
        public string? Topic { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
