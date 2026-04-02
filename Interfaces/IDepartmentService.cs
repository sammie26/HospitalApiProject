using HospitalApiProject.DTOs;
namespace HospitalApiProject.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentReadDto>> GetAllAsync();
    Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto dto);
}