using DocLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocLink.Repository
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetUpcomingAppointmentsByHospital(int hospitalId);
    }
}
