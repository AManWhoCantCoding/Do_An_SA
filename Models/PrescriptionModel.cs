using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSphere.Models
{
    public class PrescriptionModel
    {
        [Key]
        public int PrescriptionId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [Required]
        public string MedicationName { get; set; } = string.Empty;
        [Required]
        public string Dosage { get; set; } = string.Empty;
        public bool PaymentNeeded { get; set; }
        public string? Notes { get; set; }

        public PatientModel Patient { get; set; }

    }
}
