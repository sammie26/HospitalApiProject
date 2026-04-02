using Microsoft.EntityFrameworkCore;
using HospitalApiProject.Data;
using HospitalApiProject.Models;
using HospitalApiProject.DTOs;
using HospitalApiProject.Interfaces;

namespace HospitalApiProject.Services;

public class DoctorService : IDoctorService
{
    private readonly ApplicationDbContext _context;

    public DoctorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorReadDto>> GetAllDoctorsAsync()
    {
        return await _context.Doctors
            .AsNoTracking() 
            .Include(d => d.Department)
            .Select(d => new DoctorReadDto {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                DepartmentName = d.Department != null ? d.Department.Name : "General"
            })
            .ToListAsync();
    }

    public async Task<DoctorReadDto?> GetDoctorByIdAsync(int id)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Include(d => d.Department)
            .Where(d => d.Id == id)
            .Select(d => new DoctorReadDto {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                DepartmentName = d.Department != null ? d.Department.Name : "General"
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DoctorReadDto> CreateDoctorAsync(DoctorCreateDto dto)
    {
        var doctor = new Doctor { 
            Name = dto.Name, 
            Email = dto.Email, 
            DepartmentId = dto.DepartmentId 
        };
        
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        // FIX: Explicitly load the Department so we can return the Name in the DTO
        await _context.Entry(doctor).Reference(d => d.Department).LoadAsync();

        return new DoctorReadDto { 
            Id = doctor.Id, 
            Name = doctor.Name, 
            Email = doctor.Email,
            DepartmentName = doctor.Department?.Name ?? "General" // Now this won't be empty
        };
    }

    public async Task<bool> UpdateDoctorAsync(int id, DoctorUpdateDto dto)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return false;

        doctor.Name = dto.Name;
        doctor.Email = dto.Email;
        // Optimization: If your DTO includes DepartmentId, update it here too
        // doctor.DepartmentId = dto.DepartmentId; 

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteDoctorAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return false;

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
        return true;
    }
}