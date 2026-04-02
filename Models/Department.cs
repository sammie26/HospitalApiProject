namespace HospitalApiProject.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property: One department has many doctors
    public List<Doctor> Doctors { get; set; } = new(); 
}