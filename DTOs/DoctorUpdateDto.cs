using System.ComponentModel.DataAnnotations;

namespace HospitalApiProject.DTOs;

public class DoctorUpdateDto
{
    [Required(ErrorMessage = "Name is required for updates")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")] // 
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email format")] // 
    public string Email { get; set; } = string.Empty;
}