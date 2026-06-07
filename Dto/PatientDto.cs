using System.ComponentModel.DataAnnotations;

namespace DocDocGo.Dto
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool IsPrivatePatient { get; set; }
    }

    public class CreatePatientDto
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool IsPrivatePatient { get; set; }
    }
}
