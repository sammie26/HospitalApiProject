namespace HospitalApiProject.DTOs;

public class DoctorReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // This displays the string Name of the department instead of the ID
    public string DepartmentName { get; set; } = string.Empty;
}