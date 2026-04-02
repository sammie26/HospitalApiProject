using Microsoft.EntityFrameworkCore;
using HospitalApiProject.Data;
using HospitalApiProject.Models;
using HospitalApiProject.DTOs;
using HospitalApiProject.Interfaces;

namespace HospitalApiProject.Services;

public class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _context;
    public AppointmentService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<AppointmentReadDto>> GetAppointmentsAsync()
    {
        return await _context.Appointments
            .AsNoTracking() // 
            .Select(a => new AppointmentReadDto { 
                DoctorId = a.DoctorId, 
                PatientId = a.PatientId, 
                Date = a.AppointmentDate 
            })
            .ToListAsync();
    }

    public async Task<AppointmentReadDto?> GetAppointmentByIdAsync(int id)
    {
        return await _context.Appointments
            .AsNoTracking() // ✅ REQUIREMENT: Optimization
            .Where(a => a.Id == id)
            .Select(a => new AppointmentReadDto { 
                DoctorId = a.DoctorId, 
                PatientId = a.PatientId, 
                Date = a.AppointmentDate 
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> BookAsync(AppointmentCreateDto dto)
    {
        var apt = new Appointment { 
            DoctorId = dto.DoctorId, 
            PatientId = dto.PatientId, 
            AppointmentDate = dto.AppointmentDate 
        };
        _context.Appointments.Add(apt);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CancelAppointmentAsync(int id)
    {
        var a = await _context.Appointments.FindAsync(id);
        if (a == null) return false;
        
        _context.Appointments.Remove(a);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task CleanOldAppointmentsAsync()
{
    // Find all appointments where the date is in the past (before 'Now')
    var oldAppointments = await _context.Appointments
        .Where(a => a.AppointmentDate < DateTime.Now)
        .ToListAsync();

    if (oldAppointments.Any())
    {
        _context.Appointments.RemoveRange(oldAppointments);
        await _context.SaveChangesAsync();
        
        // This will show up in your Debug/Output console when the job runs
        Console.WriteLine($"[Hangfire] Successfully deleted {oldAppointments.Count} past appointments.");
    }
}
}

