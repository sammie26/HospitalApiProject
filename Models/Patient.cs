namespace HospitalApiProject.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Appointment> Appointments { get; set; } = new();
}