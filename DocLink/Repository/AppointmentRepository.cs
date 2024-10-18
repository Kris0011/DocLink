
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocLink.Models;
using System;


namespace DocLink.Repository
{
    public class AppointmentRepository:IAppointmentRepository
    {
        private readonly DocLinkDbContext _context;

        public AppointmentRepository(DocLinkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsByHospital(int hospitalId)
        {
            var upcomingAppointments = await _context.Appointments
                .Where(a => a.HospitalId == hospitalId && a.Date >= DateTime.Now) 
                .OrderBy(a => a.Date)  
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .ToListAsync();

            return upcomingAppointments;
        }


}}
