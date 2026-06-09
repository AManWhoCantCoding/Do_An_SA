using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MediSphere.Models
{
    public class UserModel : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public bool Deleted { get; set; }
        public bool AcceptedTerms { get; set; }
    }
}
