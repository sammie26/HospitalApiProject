using System.ComponentModel.DataAnnotations;

namespace HospitalApiProject.DTOs;

public class DoctorCreateDto
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")] // ✅ Requirement: MaxLength 
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")] // ✅ Requirement: EmailAddress 
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Department")] // ✅ Requirement: Range 
    public int DepartmentId { get; set; }
}