using HospitalApiProject.DTOs;

namespace HospitalApiProject.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentReadDto>> GetAppointmentsAsync();
    Task<AppointmentReadDto?> GetAppointmentByIdAsync(int id);
    Task<bool> BookAsync(AppointmentCreateDto dto);
    Task<bool> CancelAppointmentAsync(int id);
    Task CleanOldAppointmentsAsync();
}