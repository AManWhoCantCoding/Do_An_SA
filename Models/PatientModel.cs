using System.ComponentModel.DataAnnotations;

namespace DocDocGo.Models
{
    public class PatientModel
    {
        [Key]
        public int PatientId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        [EmailAddress]
        public string? EmailAddress { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public bool IsPrivatePatient { get; set; } //helps adding the future scalability for options like privatized healthcare :)
    }
}
