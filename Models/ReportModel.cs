using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocDocGo.Models
{
    public class ReportModel
    {
        [Key]
        public int ReportId { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public PatientModel? Patient { get; set; }
        public string? ReportDescription { get; set; }
        public string InitialStaffName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string Status { get; set; }
        public bool IsReportPrinted { get; set; }
        [ForeignKey("ReportType")]
        public int? ReportTypeId { get; set; }
        public ReportTypeModel? ReportTypeModel { get; set; }
    }
}
