using System.ComponentModel.DataAnnotations;

namespace HospitalApiProject.DTOs;

public class DepartmentReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class DepartmentCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}