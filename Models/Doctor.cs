namespace HospitalApiProject.Models;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Foreign Key for Department (One-to-Many)
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // ADD THIS: Navigation property for the One-to-One relationship
    public DoctorProfile? DoctorProfile { get; set; } // This fixes your CS1061 error!

    // Navigation property for Many-to-Many
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}