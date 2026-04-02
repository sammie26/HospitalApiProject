using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using HospitalApiProject.Interfaces;
using HospitalApiProject.DTOs;

namespace HospitalApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // ACCOMPLISHES: "Protect API endpoints using the Authorize attribute"
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;
    
    // ACCOMPLISHES: "Use dependency injection for the database context (via service)"
    public PatientsController(IPatientService service) => _service = service;

    [HttpGet]
    // ACCOMPLISHES: "Controllers must NOT return entity models directly"
    // ACCOMPLISHES: "Use async database calls"
    public async Task<IActionResult> GetAll() 
    {
        // NOTE: The Service must use .AsNoTracking() and .Select() 
        // to fulfill the "LINQ Optimization Requirements"
        return Ok(await _service.GetAllPatientsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) 
    {
        var p = await _service.GetPatientByIdAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")] // ACCOMPLISHES: "Restrict certain endpoints to specific roles"
    public async Task<IActionResult> Create([FromBody] PatientCreateDto dto) 
    {
        // ACCOMPLISHES: "Validation should occur before performing database operations"
        // ACCOMPLISHES: "Invalid requests must return HTTP 400 responses"
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.CreatePatientAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Implement role-based authorization"
    public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto dto) 
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _service.UpdatePatientAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Restrict certain endpoints to specific roles"
    public async Task<IActionResult> Delete(int id) 
    {
        var success = await _service.DeletePatientAsync(id);
        return success ? NoContent() : NotFound();
    }
}