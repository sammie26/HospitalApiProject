using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using HospitalApiProject.Interfaces;
using HospitalApiProject.DTOs;

namespace HospitalApiProject.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // ACCOMPLISHES: "Protect API endpoints using the Authorize attribute"
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    // ACCOMPLISHES: "Use dependency injection for the database context (via service)"
    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    // ACCOMPLISHES: "Controllers must NOT return entity models directly" (Uses DoctorReadDto)
    // ACCOMPLISHES: "Use async database calls"
    public async Task<ActionResult<IEnumerable<DoctorReadDto>>> GetDoctors()
    {
        // NOTE: Ensure Service uses .AsNoTracking() and .Select() for LINQ Optimization
        return Ok(await _doctorService.GetAllDoctorsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorReadDto>> GetDoctor(int id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        if (doctor == null) return NotFound();
        return Ok(doctor);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Restrict certain endpoints to specific roles"
    public async Task<ActionResult<DoctorReadDto>> CreateDoctor(DoctorCreateDto dto)
    {
        // ACCOMPLISHES: "Validation should occur before performing database operations"
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _doctorService.CreateDoctorAsync(dto);
        return CreatedAtAction(nameof(GetDoctor), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Implement role-based authorization"
    public async Task<IActionResult> UpdateDoctor(int id, DoctorUpdateDto dto)
    {
        // ACCOMPLISHES: "Invalid requests must return HTTP 400 responses"
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _doctorService.UpdateDoctorAsync(id, dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Restrict certain endpoints to specific roles"
    public async Task<IActionResult> DeleteDoctor(int id)
    {
        var success = await _doctorService.DeleteDoctorAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}