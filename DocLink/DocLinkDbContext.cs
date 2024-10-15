using Microsoft.EntityFrameworkCore;
using DocLink.Models;
namespace DocLink
{
    public class DocLinkDbContext : DbContext
    {
        public DocLinkDbContext(DbContextOptions<DocLinkDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Hospital) 
                .WithMany(h => h.Doctors) 
                .HasForeignKey(d => d.HospitalId) 
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)  
                .WithMany(d => d.Appointments)  
                .HasForeignKey(a => a.DoctorId)  
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient) 
                .WithMany() 
                .HasForeignKey(a => a.PatientId)  
                .OnDelete(DeleteBehavior.Restrict); 
        }




        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}