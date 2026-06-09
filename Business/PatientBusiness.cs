using MediSphere.Business.Interfaces;
using MediSphere.Dto;
using MediSphere.Models;
using MediSphere.Services.Interfaces;

namespace MediSphere.Business
{
    public class PatientBusiness : IPatientBusiness
    {
        private readonly IPatientService _patientService;

        public PatientBusiness(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _patientService.GetAllAsync();
            return patients.Select(MapToDto);
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            try
            {
                return MapToDto(await _patientService.GetByIdAsync(id));
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public async Task<BusinessResult<PatientDto>> CreateAsync(CreatePatientDto dto)
        {
            var validation = ValidatePatient(dto.FirstName, dto.LastName, dto.EmailAddress);
            if (!validation.Success)
            {
                return validation;
            }

            var patient = new PatientModel
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ContactNumber = dto.ContactNumber,
                EmailAddress = dto.EmailAddress,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                IsPrivatePatient = dto.IsPrivatePatient
            };

            var created = await _patientService.CreateAsync(patient);
            return BusinessResult<PatientDto>.Ok(MapToDto(created));
        }

        public async Task<BusinessResult<PatientDto>> UpdateAsync(int id, CreatePatientDto dto)
        {
            var validation = ValidatePatient(dto.FirstName, dto.LastName, dto.EmailAddress);
            if (!validation.Success)
            {
                return validation;
            }

            try
            {
                var existing = await _patientService.GetByIdAsync(id);
                existing.FirstName = dto.FirstName;
                existing.LastName = dto.LastName;
                existing.ContactNumber = dto.ContactNumber;
                existing.EmailAddress = dto.EmailAddress;
                existing.DateOfBirth = dto.DateOfBirth;
                existing.Gender = dto.Gender;
                existing.IsPrivatePatient = dto.IsPrivatePatient;

                var updated = await _patientService.UpdateAsync(existing);
                return BusinessResult<PatientDto>.Ok(MapToDto(updated));
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<PatientDto>.Fail("Patient not found.");
            }
        }

        public async Task<BusinessResult<bool>> DeleteAsync(int id)
        {
            try
            {
                await _patientService.DeleteAsync(id);
                return BusinessResult<bool>.Ok(true);
            }
            catch (KeyNotFoundException)
            {
                return BusinessResult<bool>.Fail("Patient not found.");
            }
        }

        private static BusinessResult<PatientDto> ValidatePatient(string? firstName, string? lastName, string? email)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return BusinessResult<PatientDto>.Fail("First name and last name are required.");
            }

            if (!string.IsNullOrWhiteSpace(email) && !email.Contains('@'))
            {
                return BusinessResult<PatientDto>.Fail("Email address is not valid.");
            }

            return BusinessResult<PatientDto>.Ok(null!);
        }

        private static PatientDto MapToDto(PatientModel patient) => new()
        {
            PatientId = patient.PatientId,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            ContactNumber = patient.ContactNumber,
            EmailAddress = patient.EmailAddress,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            IsPrivatePatient = patient.IsPrivatePatient
        };
    }
}
