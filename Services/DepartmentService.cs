using Microsoft.EntityFrameworkCore;
using HospitalApiProject.Data;
using HospitalApiProject.Models;
using HospitalApiProject.DTOs;
using HospitalApiProject.Interfaces;
namespace HospitalApiProject.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;
    public DepartmentService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<DepartmentReadDto>> GetAllAsync() => 
        await _context.Departments
            .AsNoTracking() // REQUIREMENT: Optimization 
            .Select(d => new DepartmentReadDto { Id = d.Id, Name = d.Name })
            .ToListAsync();

    public async Task<DepartmentReadDto> CreateAsync(DepartmentCreateDto dto)
    {
        var dep = new Department { Name = dto.Name };
        _context.Departments.Add(dep);
        await _context.SaveChangesAsync();
        return new DepartmentReadDto { Id = dep.Id, Name = dep.Name };
    }
}