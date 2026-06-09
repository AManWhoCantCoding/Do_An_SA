using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using MediSphere.Models;
using MediSphere.Services.Interfaces;

namespace MediSphere.Business
{
    public class PrescriptionBusiness : IPrescriptionBusiness
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IPatientService _patientService;

        public PrescriptionBusiness(IPrescriptionService prescriptionService, IPatientService patientService)
        {
            _prescriptionService = prescriptionService;
            _patientService = patientService;
        }

        public async Task<IEnumerable<PrescriptionDto>> GetAllAsync()
        {
            var prescriptions = await _prescriptionService.GetAllAsync();
            return prescriptions.Select(MapToDto);
        }

        public async Task<PrescriptionDto?> GetByIdAsync(int id)
        {
            try
            {
                return MapToDto(await _prescriptionService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<PrescriptionDto>> GetByPatientIdAsync(int patientId)
        {
            var prescriptions = await _prescriptionService.GetByPatientIdAsync(patientId);
            return prescriptions.Select(MapToDto);
        }

        public async Task<BusinessResult<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto)
        {
            var validation = await ValidatePrescriptionAsync(dto);
            if (!validation.Success)
            {
                return validation;
            }

            var prescription = new PrescriptionModel
            {
                PatientId = dto.PatientId,
                MedicationName = dto.MedicationName,
                Dosage = dto.Dosage,
                PaymentNeeded = dto.PaymentNeeded,
                Notes = dto.Notes
            };

            var created = await _prescriptionService.CreateAsync(prescription);
            return BusinessResult<PrescriptionDto>.Ok(MapToDto(created));
        }

        public async Task<BusinessResult<PrescriptionDto>> UpdateAsync(int id, CreatePrescriptionDto dto)
        {
            var validation = await ValidatePrescriptionAsync(dto);
            if (!validation.Success)
            {
                return validation;
            }

            try
            {
                var existing = await _prescriptionService.GetByIdAsync(id);
                existing.PatientId = dto.PatientId;
                existing.MedicationName = dto.MedicationName;
                existing.Dosage = dto.Dosage;
                existing.PaymentNeeded = dto.PaymentNeeded;
                existing.Notes = dto.Notes;

                var updated = await _prescriptionService.UpdateAsync(existing);
                return BusinessResult<PrescriptionDto>.Ok(MapToDto(updated));
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<PrescriptionDto>.Fail("Prescription not found.");
            }
        }

        public async Task<BusinessResult<bool>> DeleteAsync(int id)
        {
            try
            {
                await _prescriptionService.DeleteAsync(id);
                return BusinessResult<bool>.Ok(true);
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<bool>.Fail("Prescription not found.");
            }
        }

        private async Task<BusinessResult<PrescriptionDto>> ValidatePrescriptionAsync(CreatePrescriptionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MedicationName) || string.IsNullOrWhiteSpace(dto.Dosage))
            {
                return BusinessResult<PrescriptionDto>.Fail("Medication name and dosage are required.");
            }

            if (!await _patientService.ExistsAsync(dto.PatientId))
            {
                return BusinessResult<PrescriptionDto>.Fail("Patient does not exist.");
            }

            return BusinessResult<PrescriptionDto>.Ok(null!);
        }

        private static PrescriptionDto MapToDto(PrescriptionModel prescription) => new()
        {
            PrescriptionId = prescription.PrescriptionId,
            PatientId = prescription.PatientId,
            MedicationName = prescription.MedicationName,
            Dosage = prescription.Dosage,
            PaymentNeeded = prescription.PaymentNeeded,
            Notes = prescription.Notes
        };
    }
}
