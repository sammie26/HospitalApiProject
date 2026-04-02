namespace HospitalApiProject.Models;

public class DoctorProfile
{
    public int Id { get; set; }
    public string Bio { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }

    // Foreign Key
    public int DoctorId { get; set; }
    // Reference Navigation Property
    public Doctor Doctor { get; set; } = null!;
}