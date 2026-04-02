using System.ComponentModel.DataAnnotations;

namespace HospitalApiProject.DTOs;

public class PatientUpdateDto
{
    [Required(ErrorMessage = "Patient name is required for updates")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;
}