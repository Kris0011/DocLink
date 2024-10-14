

using DocLink.Models;
using Microsoft.EntityFrameworkCore;

public class DocLinkDbContext : DbContext
{
    public DocLinkDbContext(DbContextOptions<DocLinkDbContext> options)
        : base(options)
    {

    }

    public DbSet<Patient> Patients {  get; set; }
    public DbSet<Doctor> Doctors { get; set; }

    public DbSet<Appointment> Appointments { get; set; }

    public DbSet<Hospital> Hospitals { get; set; }



}
