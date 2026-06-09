using System.ComponentModel.DataAnnotations;

namespace MediSphere.Models
{
    public class ReportTypeModel
    {
        [Key]
        public int ReportTypeId { get; set; }
        [Required]
        public string TemplateType { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        public DateTime ReportTypeCreationTime { get; set; }
    }
}
