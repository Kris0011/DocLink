using System;
using System.Collections.Generic;
using DocLink.Models;
using System.Linq;

namespace DocLink.Repository;

public class HospitalRepository : IHospitalRepository
{
    private readonly DocLinkDbContext _context;

    public HospitalRepository(DocLinkDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Hospital> GetHospitals()
    {
        return _context.Hospitals;
    }


    public IEnumerable<Doctor> GetDoctorByHospitalId(int hospitalId)
    {
      return  _context.Doctors.Where(d => d.HospitalId == hospitalId);
    }

    public Hospital GetHospitalById(int hospitalId)
    {
        Hospital hospital = _context.Hospitals.Find(hospitalId);
        return hospital;
    }
}