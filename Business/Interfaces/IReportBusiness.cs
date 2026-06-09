using MediSphere.Business;
using MediSphere.Dto;
using MediSphere.Models;

namespace MediSphere.Business.Interfaces
{
    public interface IReportBusiness
    {
        Task<IEnumerable<ReportDto>> GetAllAsync();
        Task<ReportDto?> GetByIdAsync(int id);
        Task<BusinessResult<ReportModel>> CreateReportAsync(ReportModel report);
        Task<BusinessResult<ReportModel>> UpdateReportAsync(ReportModel report, string? updatedBy);
        Task<BusinessResult<ReportTypeModel>> CreateReportTypeAsync(ReportTypeModel reportType);
        Task<IEnumerable<ReportTypeModel>> GetReportTypesAsync();
        Task<IEnumerable<PatientModel>> GetPatientsForReportsAsync();
        Task<BusinessResult<bool>> MarkAsPrintedAsync(int reportId);
    }
}
