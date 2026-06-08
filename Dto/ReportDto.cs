namespace MediSphere.Dto
{
    public class ReportDto
    {
        public int ReportId { get; set; }
        public int PatientId { get; set; }
        public string? ReportDescription { get; set; }
        public string InitialStaffName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsReportPrinted { get; set; }
        public int? ReportTypeId { get; set; }
        public string? ReportTypeName { get; set; }
    }
}
