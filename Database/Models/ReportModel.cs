using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSphere.Models
{
    public class ReportModel
    {
        [Key]
        public int ReportId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public PatientModel? Patient { get; set; }
        public string? ReportDescription { get; set; }
        public string InitialStaffName { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsReportPrinted { get; set; }
        public int? ReportTypeId { get; set; }
        public ReportTypeModel? ReportTypeModel { get; set; }
    }
}
