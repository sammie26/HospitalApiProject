using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using HospitalApiProject.Interfaces;
using HospitalApiProject.DTOs;

namespace HospitalApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // ACCOMPLISHES: "Protect API endpoints using the Authorize attribute"
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;

    // ACCOMPLISHES: "Use dependency injection for the database context (via service)"
    public DepartmentsController(IDepartmentService service) => _service = service;

    [HttpGet]
    // ACCOMPLISHES: "Controllers must NOT return entity models directly"
    // ACCOMPLISHES: "Use async database calls"
    public async Task<IActionResult> GetAll() 
    {
        // NOTE: The Service must use .AsNoTracking() and .Select() 
        // to fulfill the "LINQ Optimization Requirements"
        var departments = await _service.GetAllAsync();
        return Ok(departments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Restrict certain endpoints to specific roles"
    public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto) 
    {
        // ACCOMPLISHES: "Validate incoming requests using DTO validation"
        // ACCOMPLISHES: "Invalid requests must return HTTP 400 responses"
        // ACCOMPLISHES: "Validation should occur before performing database operations"
        if (!ModelState.IsValid) 
        {
            return BadRequest(ModelState);
        }

        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }
}