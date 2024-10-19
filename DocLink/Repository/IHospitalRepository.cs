using System.Collections.Generic;
using DocLink.Models;

namespace DocLink.Repository;

public interface IHospitalRepository
{
    IEnumerable<Hospital> GetHospitals();
    public IEnumerable<Doctor> GetDoctorByHospitalId(int hospitalId);
    
    public Hospital GetHospitalById(int id);

}