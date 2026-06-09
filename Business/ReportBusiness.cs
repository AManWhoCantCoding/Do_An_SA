using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using MediSphere.Models;
using MediSphere.Services.Interfaces;

namespace MediSphere.Business
{
    public class ReportBusiness : IReportBusiness
    {
        private readonly IReportService _reportService;
        private readonly IReportTypeService _reportTypeService;
        private readonly IPatientService _patientService;
        private readonly INotificationService _notificationService;

        public ReportBusiness(
            IReportService reportService,
            IReportTypeService reportTypeService,
            IPatientService patientService,
            INotificationService notificationService)
        {
            _reportService = reportService;
            _reportTypeService = reportTypeService;
            _patientService = patientService;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            var reports = await _reportService.GetAllAsync();
            return reports.Select(MapToDto);
        }

        public async Task<ReportDto?> GetByIdAsync(int id)
        {
            try
            {
                return MapToDto(await _reportService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public async Task<BusinessResult<ReportModel>> CreateReportAsync(ReportModel report)
        {
            if (string.IsNullOrWhiteSpace(report.InitialStaffName))
            {
                return BusinessResult<ReportModel>.Fail("Staff name is required.");
            }

            if (!await _patientService.ExistsAsync(report.PatientId))
            {
                return BusinessResult<ReportModel>.Fail("Patient does not exist.");
            }

            report.CreatedAt = DateTime.Now;
            report.LastUpdated = DateTime.Now;
            report.Status ??= "Draft";

            if (report.ReportTypeId.HasValue)
            {
                try
                {
                    await _reportTypeService.GetByIdAsync(report.ReportTypeId.Value);
                }
                catch (KeyNotFoundException)
                {
                    return BusinessResult<ReportModel>.Fail("Selected report type does not exist.");
                }
            }

            var created = await _reportService.CreateAsync(report);
            return BusinessResult<ReportModel>.Ok(created);
        }

        public async Task<BusinessResult<ReportModel>> UpdateReportAsync(ReportModel report, string? updatedBy)
        {
            try
            {
                var existing = await _reportService.GetByIdAsync(report.ReportId);
                existing.ReportDescription = report.ReportDescription;
                existing.PatientId = report.PatientId;
                existing.InitialStaffName = report.InitialStaffName;
                existing.Status = report.Status;
                existing.LastUpdated = DateTime.Now;
                existing.LastUpdatedBy = updatedBy;

                var updated = await _reportService.UpdateAsync(existing);

                try
                {
                    var patient = await _patientService.GetByIdAsync(updated.PatientId);
                    var patientName = $"{patient.FirstName} {patient.LastName}".Trim();
                    await _notificationService.SendReportStatusUpdateAsync(
                        patient.EmailAddress ?? string.Empty,
                        patientName,
                        updated.Status);
                }
                catch (Exception)
                {
                    // Email failure must not block report update.
                }

                return BusinessResult<ReportModel>.Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<ReportModel>.Fail("Report not found.");
            }
        }

        public async Task<BusinessResult<ReportTypeModel>> CreateReportTypeAsync(ReportTypeModel reportType)
        {
            if (string.IsNullOrWhiteSpace(reportType.TemplateType))
            {
                return BusinessResult<ReportTypeModel>.Fail("Template type is required.");
            }

            reportType.ReportTypeCreationTime = DateTime.Now;
            var created = await _reportTypeService.CreateAsync(reportType);
            return BusinessResult<ReportTypeModel>.Ok(created);
        }

        public Task<IEnumerable<ReportTypeModel>> GetReportTypesAsync() => _reportTypeService.GetAllAsync();

        public async Task<int?> ResolveReportTypeIdAsync(int? selectedTemplateTypeId, string? reportTypeName)
        {
            if (selectedTemplateTypeId.HasValue)
            {
                return selectedTemplateTypeId;
            }

            if (string.IsNullOrWhiteSpace(reportTypeName))
            {
                return null;
            }

            var trimmedName = reportTypeName.Trim();
            var types = await _reportTypeService.GetAllAsync();
            var existing = types.FirstOrDefault(t =>
                string.Equals(t.TemplateType, trimmedName, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                return existing.ReportTypeId;
            }

            var created = await _reportTypeService.CreateAsync(new ReportTypeModel
            {
                TemplateType = trimmedName,
                ReportTypeCreationTime = DateTime.Now
            });

            return created.ReportTypeId;
        }

        public Task<IEnumerable<PatientModel>> GetPatientsForReportsAsync() => _patientService.GetAllAsync();

        public async Task<BusinessResult<bool>> MarkAsPrintedAsync(int reportId)
        {
            try
            {
                await _reportService.MarkAsPrintedAsync(reportId);
                return BusinessResult<bool>.Ok(true);
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<bool>.Fail("Report not found.");
            }
        }

        private static ReportDto MapToDto(ReportModel report) => new()
        {
            ReportId = report.ReportId,
            PatientId = report.PatientId,
            ReportDescription = report.ReportDescription,
            InitialStaffName = report.InitialStaffName,
            CreatedAt = report.CreatedAt,
            LastUpdated = report.LastUpdated,
            LastUpdatedBy = report.LastUpdatedBy,
            Status = report.Status,
            IsReportPrinted = report.IsReportPrinted,
            ReportTypeId = report.ReportTypeId,
            ReportTypeName = report.ReportTypeModel?.TemplateType
        };
    }
}
