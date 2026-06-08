using System.ComponentModel.DataAnnotations;

namespace MediSphere.Dto
{
    public class PrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public int PatientId { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public bool PaymentNeeded { get; set; }
        public string? Notes { get; set; }
    }

    public class CreatePrescriptionDto
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public string MedicationName { get; set; } = string.Empty;
        [Required]
        public string Dosage { get; set; } = string.Empty;
        public bool PaymentNeeded { get; set; }
        public string? Notes { get; set; }
    }
}
