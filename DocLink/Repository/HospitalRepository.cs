using System.Collections.Generic;
using DocLink.Models;

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
}