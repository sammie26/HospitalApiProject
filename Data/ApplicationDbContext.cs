using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HospitalApiProject.Models;

namespace HospitalApiProject.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) 
    { 
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DoctorProfile> DoctorProfiles { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder) is mandatory for Identity to work
        base.OnModelCreating(modelBuilder);

        // 1. One-to-One: Doctor <-> DoctorProfile 
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.DoctorProfile) 
            .WithOne(p => p.Doctor)
            .HasForeignKey<DoctorProfile>(p => p.DoctorId);

        // 2. Many-to-Many: Doctor <-> Patient via Appointment 
        // CHANGED: We now use 'Id' as the Primary Key so it becomes an IDENTITY column in SQL
        modelBuilder.Entity<Appointment>()
            .HasKey(a => a.Id); 

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId);
            
        // 3. One-to-Many: Department <-> Doctor 
        modelBuilder.Entity<Department>()
            .HasMany(dep => dep.Doctors)
            .WithOne(doc => doc.Department)
            .HasForeignKey(doc => doc.DepartmentId);
    }
}