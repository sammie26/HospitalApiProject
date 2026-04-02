using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize] [cite: 39]
using HospitalApiProject.Interfaces;
using HospitalApiProject.DTOs;

namespace HospitalApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // ACCOMPLISHES: "Protect API endpoints using the Authorize attribute" [cite: 14, 39]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _service;

    // ACCOMPLISHES: "Use dependency injection for the database context (via service)" [cite: 9, 23]
    public AppointmentsController(IAppointmentService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetHistory() 
    {
        // ACCOMPLISHES: "Controllers must NOT return entity models directly" [cite: 28]
        // ACCOMPLISHES: "Optimize queries using LINQ Select projections (via service)" [cite: 15, 44]
        // ACCOMPLISHES: "Use async database calls" 
        var history = await _service.GetAppointmentsAsync();
        return Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> Book([FromBody] AppointmentCreateDto dto) 
    {
        // ACCOMPLISHES: "Validate incoming requests using DTO validation" [cite: 12]
        // ACCOMPLISHES: "Invalid requests must return HTTP 400 responses" [cite: 31]
        // ACCOMPLISHES: "Validation should occur before performing database operations" [cite: 32]
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _service.BookAsync(dto);
        
        if (!result) return BadRequest("Could not complete the booking. Please check Doctor/Patient IDs.");

        return Ok(new { message = "Appointment booked successfully" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // ACCOMPLISHES: "Implement role-based authorization" [cite: 40]
    // ACCOMPLISHES: "Restrict certain endpoints to specific roles" [cite: 42]
    public async Task<IActionResult> Cancel(int id) 
    {
        var result = await _service.CancelAppointmentAsync(id);
        return result ? NoContent() : NotFound(new { error = $"Appointment with ID {id} not found." });
    }
}