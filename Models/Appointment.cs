using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalApiProject.Models;

[Table("Appointments")] // This forces SQL to look for "Appointments"
public class Appointment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    // Optional: Useful for your Hangfire cleanup logic
    public string Status { get; set; } = "Pending"; 

    // Foreign Keys
    [Required]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    [Required]
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}