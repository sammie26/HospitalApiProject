using Microsoft.EntityFrameworkCore;
using HospitalApiProject.Data;
using HospitalApiProject.Models;
using HospitalApiProject.DTOs;
using HospitalApiProject.Interfaces;
namespace HospitalApiProject.Services;

public class PatientService : IPatientService
{
    private readonly ApplicationDbContext _context;

    public PatientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientReadDto>> GetAllPatientsAsync()
    {
        // Optimization: Use AsNoTracking for read-only queries
        return await _context.Patients
            .AsNoTracking()
            .Select(p => new PatientReadDto 
            { 
                Id = p.Id, 
                Name = p.Name 
            })
            .ToListAsync();
    }

    public async Task<PatientReadDto?> GetPatientByIdAsync(int id)
    {
        // Optimization: Use AsNoTracking for read-only queries
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PatientReadDto 
            { 
                Id = p.Id, 
                Name = p.Name 
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PatientReadDto> CreatePatientAsync(PatientCreateDto dto)
    {
        var patient = new Patient 
        { 
            Name = dto.Name 
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return new PatientReadDto 
        { 
            Id = patient.Id, 
            Name = patient.Name 
        };
    }

    public async Task<bool> UpdatePatientAsync(int id, PatientUpdateDto dto)
    {
        var p = await _context.Patients.FindAsync(id);
        if (p == null) return false;

        p.Name = dto.Name;
        
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        var p = await _context.Patients.FindAsync(id);
        if (p == null) return false;

        _context.Patients.Remove(p);
        return await _context.SaveChangesAsync() > 0;
    }
}