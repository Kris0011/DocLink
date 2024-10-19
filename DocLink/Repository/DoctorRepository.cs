using System.Collections.Generic;
using DocLink.Models;

namespace DocLink.Repository;

public class DoctorRepository : IDoctorRepository
{
    private readonly DocLinkDbContext _context;
    DoctorRepository(DocLinkDbContext context)
    {
        _context = context;
    }
    
    public Doctor GetDoctorById(int id)
    {
        return _context.Doctors.Find(id);
    }
    
    public IEnumerable<Doctor> GetDoctors()
    {
        return _context.Doctors;
    }
    
    
}