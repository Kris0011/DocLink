using System.Collections.Generic;
using DocLink.Models;

namespace DocLink.Repository;

public interface IDoctorRepository
{
    public Doctor GetDoctorById(int id);
    
    public IEnumerable<Doctor> GetDoctors();
    
}