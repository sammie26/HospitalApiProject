namespace HospitalApiProject.DTOs;

public class AppointmentReadDto
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime Date { get; set; }
}