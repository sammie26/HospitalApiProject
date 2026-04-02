using HospitalApiProject.DTOs;

namespace HospitalApiProject.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientReadDto>> GetAllPatientsAsync();
    Task<PatientReadDto?> GetPatientByIdAsync(int id);
    Task<PatientReadDto> CreatePatientAsync(PatientCreateDto dto);
    Task<bool> UpdatePatientAsync(int id, PatientUpdateDto dto);
    Task<bool> DeletePatientAsync(int id);
}