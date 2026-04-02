using System.ComponentModel.DataAnnotations;

namespace HospitalApiProject.DTOs;

public class AppointmentCreateDto
{
    [Required(ErrorMessage = "Doctor ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Doctor")]
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "Patient ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Patient")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Appointment date and time are required")]
    [DataType(DataType.DateTime)]
    public DateTime AppointmentDate { get; set; }
}