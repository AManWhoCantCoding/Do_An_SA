using System.ComponentModel.DataAnnotations;

namespace DocDocGo.Models
{
    public class ReportTypeModel
    {
        [Key]
        public int ReportTypeId { get; set; }
        [Required]
        public string TemplateType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReportTypeCreationTime { get; set; }
    }
}
