using HospitalApiProject.DTOs;

namespace HospitalApiProject.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorReadDto>> GetAllDoctorsAsync();
    Task<DoctorReadDto?> GetDoctorByIdAsync(int id);
    Task<DoctorReadDto> CreateDoctorAsync(DoctorCreateDto doctorCreateDto);
    Task<bool> UpdateDoctorAsync(int id, DoctorUpdateDto doctorUpdateDto);
    Task<bool> DeleteDoctorAsync(int id);
}